using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Courses.DTOs
{
    public class CreateCourseRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile? ThumbnailFile { get; set; }

        public bool IsPublished { get; set; } = false;
    }
} 