using MediatR;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.CourseRatings.Commands.AddCourseRating
{
    public class AddCourseRatingCommand : IRequest<int>
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }

        [Range(1, 5)]
        public int RatingValue { get; set; }
        public string? ReviewText { get; set; }
    }
} 