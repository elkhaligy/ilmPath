using Google.Cloud.Storage.V1;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;

namespace IlmPath.Infrastructure.Services
{
    public class GoogleCloudStorageService : IVideoUploadService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly ILogger<GoogleCloudStorageService> _logger;

        public GoogleCloudStorageService(IConfiguration configuration, ILogger<GoogleCloudStorageService> logger)
        {
            _logger = logger;
            
            // Get configuration values
            var credentialsPath = configuration["GoogleCloud:CredentialsPath"];
            _bucketName = configuration["GoogleCloud:StorageBucketName"] 
                ?? throw new InvalidOperationException("GoogleCloud:StorageBucketName is not configured");
            
            // Initialize Google Cloud Storage client with credentials
            if (!string.IsNullOrEmpty(credentialsPath))
            {
                if (!File.Exists(credentialsPath))
                {
                    throw new FileNotFoundException($"Google Cloud credentials file not found: {credentialsPath}");
                }
                
                // Create client with explicit credentials
                _storageClient = StorageClient.Create(Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(credentialsPath));
                _logger.LogInformation($"Google Cloud Storage client initialized with credentials from: {credentialsPath}");
            }
            else
            {
                // Fallback to default credentials (environment variable)
                _storageClient = StorageClient.Create();
                _logger.LogInformation("Google Cloud Storage client initialized with default credentials");
            }
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile, string lectureId)
        {
            try
            {
                // Validate file
                if (videoFile == null || videoFile.Length == 0)
                {
                    throw new ArgumentException("Video file is null or empty");
                }

                // Validate file type
                var allowedTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm" };
                if (!allowedTypes.Contains(videoFile.ContentType?.ToLower()))
                {
                    throw new ArgumentException($"Unsupported video format: {videoFile.ContentType}");
                }

                // Generate unique object name
                var fileExtension = Path.GetExtension(videoFile.FileName);
                var objectName = $"videos/lectures/{lectureId}/{Guid.NewGuid()}{fileExtension}";

                _logger.LogInformation($"Uploading video to GCS: {objectName}");

                // Upload to Google Cloud Storage
                using var stream = videoFile.OpenReadStream();
                
                var storageObject = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: objectName,
                    contentType: videoFile.ContentType,
                    source: stream
                );

                // Generate public URL
                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{objectName}";
                
                _logger.LogInformation($"Video uploaded successfully: {publicUrl}");
                
                return publicUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video to Google Cloud Storage");
                throw;
            }
        }

        public async Task<VideoUploadResult> UploadVideoWithDurationAsync(IFormFile videoFile, string lectureId)
        {
            string tempFilePath = string.Empty;
            
            try
            {
                // Validate file
                if (videoFile == null || videoFile.Length == 0)
                {
                    throw new ArgumentException("Video file is null or empty");
                }

                // Validate file type
                var allowedTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm" };
                if (!allowedTypes.Contains(videoFile.ContentType?.ToLower()))
                {
                    throw new ArgumentException($"Unsupported video format: {videoFile.ContentType}");
                }

                _logger.LogInformation($"Starting video upload with duration extraction for lecture {lectureId}");

                // Create temporary file
                var fileExtension = Path.GetExtension(videoFile.FileName);
                tempFilePath = Path.Combine(Path.GetTempPath(), $"video_{Guid.NewGuid()}{fileExtension}");

                _logger.LogInformation($"Saving video to temporary file: {tempFilePath}");

                // Save video file temporarily
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                }

                _logger.LogInformation($"Video saved temporarily, extracting duration from: {tempFilePath}");

                // Extract video duration using FFMpegCore
                int durationInMinutes = 0;
                double durationInSeconds = 0;

                try
                {
                    var mediaInfo = await FFProbe.AnalyseAsync(tempFilePath);
                    durationInSeconds = mediaInfo.Duration.TotalSeconds;
                    durationInMinutes = (int)Math.Ceiling(mediaInfo.Duration.TotalMinutes);

                    _logger.LogInformation($"Video duration extracted: {durationInSeconds:F2} seconds ({durationInMinutes} minutes)");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to extract video duration, defaulting to 0");
                    // Continue with upload even if duration extraction fails
                }

                // Generate unique object name for GCS
                var objectName = $"videos/lectures/{lectureId}/{Guid.NewGuid()}{fileExtension}";

                _logger.LogInformation($"Uploading video to GCS: {objectName}");

                // Upload to Google Cloud Storage from temporary file
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                {
                    var storageObject = await _storageClient.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: videoFile.ContentType,
                        source: fileStream
                    );
                }

                // Generate public URL
                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{objectName}";
                
                _logger.LogInformation($"Video uploaded successfully with duration extraction: {publicUrl}");

                return new VideoUploadResult
                {
                    VideoUrl = publicUrl,
                    DurationInMinutes = durationInMinutes,
                    DurationInSeconds = durationInSeconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video with duration extraction to Google Cloud Storage");
                throw;
            }
            finally
            {
                // Clean up temporary file
                if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                        _logger.LogInformation($"Temporary file deleted: {tempFilePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Failed to delete temporary file: {tempFilePath}");
                    }
                }
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                // Validate parameters
                if (fileStream == null || fileStream.Length == 0)
                {
                    throw new ArgumentException("File stream is null or empty");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentException("File name cannot be null or empty");
                }

                _logger.LogInformation($"Uploading file to GCS: {fileName}");

                // Upload to Google Cloud Storage
                var storageObject = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: fileName,
                    contentType: contentType,
                    source: fileStream
                );

                // Generate public URL
                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";
                
                _logger.LogInformation($"File uploaded successfully: {publicUrl}");
                
                return publicUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to Google Cloud Storage");
                throw;
            }
        }

        public async Task<bool> DeleteVideoAsync(string videoUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(videoUrl))
                {
                    return true; // Nothing to delete
                }

                // Extract object name from URL
                var uri = new Uri(videoUrl);
                var objectName = uri.AbsolutePath.TrimStart('/');
                
                // Remove bucket name from the path if present
                if (objectName.StartsWith($"{_bucketName}/"))
                {
                    objectName = objectName.Substring($"{_bucketName}/".Length);
                }

                _logger.LogInformation($"Deleting video from GCS: {objectName}");

                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
                
                _logger.LogInformation($"Video deleted successfully: {objectName}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting video from Google Cloud Storage");
                return false;
            }
        }
    }
} 