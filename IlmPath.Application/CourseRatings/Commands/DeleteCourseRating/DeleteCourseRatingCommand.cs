using MediatR;

namespace IlmPath.Application.CourseRatings.Commands.DeleteCourseRating
{
    public class DeleteCourseRatingCommand : IRequest
    {
        public int RatingId { get; set; }
        public string UserId { get; set; }
    }
} 