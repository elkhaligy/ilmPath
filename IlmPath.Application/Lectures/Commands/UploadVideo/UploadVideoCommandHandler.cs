using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.Commands.UploadVideo
{
    public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, UploadVideoResponse>
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IVideoUploadService _videoUploadService;
        private readonly ILogger<UploadVideoCommandHandler> _logger;

        public UploadVideoCommandHandler(
            ILectureRepository lectureRepository,
            IVideoUploadService videoUploadService,
            ILogger<UploadVideoCommandHandler> logger)
        {
            _lectureRepository = lectureRepository;
            _videoUploadService = videoUploadService;
            _logger = logger;
        }

        public async Task<UploadVideoResponse> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the lecture
                var lecture = await _lectureRepository.GetByIdAsync(request.LectureId);

                if (lecture == null)
                {
                    throw new NotFoundException(nameof(lecture), request.LectureId);
                }

                _logger.LogInformation($"Uploading video with duration extraction for lecture {request.LectureId}");

                // Upload video to Google Cloud Storage with duration extraction
                var uploadResult = await _videoUploadService.UploadVideoWithDurationAsync(
                    request.VideoFile, 
                    request.LectureId.ToString());

                // Update lecture with new video URL and duration
                lecture.VideoUrl = uploadResult.VideoUrl;
                lecture.DurationInMinutes = uploadResult.DurationInMinutes;

                await _lectureRepository.UpdateAsync(lecture);

                _logger.LogInformation($"Video uploaded successfully for lecture {request.LectureId}: {uploadResult.VideoUrl}, Duration: {uploadResult.DurationInMinutes} minutes");

                return new UploadVideoResponse
                {
                    VideoUrl = uploadResult.VideoUrl,
                    Message = "Video uploaded successfully",
                    DurationInMinutes = uploadResult.DurationInMinutes,
                    DurationInSeconds = uploadResult.DurationInSeconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading video for lecture {request.LectureId}");
                throw;
            }
        }
    }
} 