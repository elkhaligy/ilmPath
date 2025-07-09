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

                _logger.LogInformation($"Uploading video for lecture {request.LectureId}");

                // Upload video to Google Cloud Storage
                var videoUrl = await _videoUploadService.UploadVideoAsync(
                    request.VideoFile, 
                    request.LectureId.ToString());

                // Update lecture with new video URL
                lecture.VideoUrl = videoUrl;

                await _lectureRepository.UpdateAsync(lecture);

                _logger.LogInformation($"Video uploaded successfully for lecture {request.LectureId}: {videoUrl}");

                return new UploadVideoResponse
                {
                    VideoUrl = videoUrl,
                    Message = "Video uploaded successfully"
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