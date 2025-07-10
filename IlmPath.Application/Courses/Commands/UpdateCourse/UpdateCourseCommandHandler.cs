using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlmPath.Application.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler(ICourseRepository _courseRepository, IMapper _mapper, IVideoUploadService _uploadService) : IRequestHandler<UpdateCourseCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var courseToUpdate = await _courseRepository.GetByIdAsync(request.Id);

            if (courseToUpdate == null)
            {
                return Unit.Value;
            }

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

                // Update the thumbnail URL in the course
                courseToUpdate.ThumbnailImageUrl = thumbnailUrl;
            }
            else if (!string.IsNullOrEmpty(request.ThumbnailImageUrl))
            {
                // Use provided thumbnail URL if no file is uploaded
                courseToUpdate.ThumbnailImageUrl = request.ThumbnailImageUrl;
            }

            // Map other properties
            courseToUpdate.Title = request.Title;
            courseToUpdate.Description = request.Description;
            courseToUpdate.Price = request.Price;
            courseToUpdate.IsPublished = request.IsPublished;
            courseToUpdate.CategoryId = request.CategoryId;
            courseToUpdate.UpdatedAt = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(courseToUpdate);

            return Unit.Value;
        }
    }
}
