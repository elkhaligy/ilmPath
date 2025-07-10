using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.CourseRatings.DTOs.Requests
{
    public class AddCourseRatingRequest
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }

        public string? ReviewText { get; set; }
    }
} 