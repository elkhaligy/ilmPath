using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlmPath.Application.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IVideoUploadService _uploadService;

        public CreateCourseCommandHandler(
            ICourseRepository courseRepository, 
            IMapper mapper,
            IVideoUploadService uploadService)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<CourseResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = _mapper.Map<Course>(request);
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;

            // Handle thumbnail upload if provided
            if (request.ThumbnailFile != null)
            {
                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(request.ThumbnailFile.ContentType.ToLower()))
                {
                    throw new ArgumentException("Invalid file type. Only JPEG, PNG, and WebP images are allowed.");
                }

                // Validate file size (max 5MB)
                const int maxSizeBytes = 5 * 1024 * 1024; // 5MB
                if (request.ThumbnailFile.Length > maxSizeBytes)
                {
                    throw new ArgumentException("File size exceeds the 5MB limit.");
                }

                // Generate unique filename
                var fileExtension = Path.GetExtension(request.ThumbnailFile.FileName);
                var uniqueFileName = $"thumbnails/{Guid.NewGuid()}{fileExtension}";

                // Upload to Google Cloud Storage
                var thumbnailUrl = await _uploadService.UploadFileAsync(
                    request.ThumbnailFile.OpenReadStream(), 
                    uniqueFileName, 
                    request.ThumbnailFile.ContentType);

                course.ThumbnailImageUrl = thumbnailUrl;
            }

            var newCourse = await _courseRepository.AddAsync(course);

            return _mapper.Map<CourseResponse>(newCourse);
        }
    }
}
