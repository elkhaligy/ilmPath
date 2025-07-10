using System;

namespace IlmPath.Application.CourseRatings.DTOs.Responses
{
    public class CourseRatingResponse
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string? ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
    }
} 