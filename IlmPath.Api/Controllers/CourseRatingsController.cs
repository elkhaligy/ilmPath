using IlmPath.Application.Common.Pagination;
using IlmPath.Application.CourseRatings.Commands.AddCourseRating;
using IlmPath.Application.CourseRatings.Commands.DeleteCourseRating;
using IlmPath.Application.CourseRatings.DTOs.Requests;
using IlmPath.Application.CourseRatings.DTOs.Responses;
using IlmPath.Application.CourseRatings.Queries.GetCourseRatings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseRatingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseRatingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a paginated list of ratings for a specific course.
        /// </summary>
        /// <remarks>
        /// This endpoint is publicly accessible and can be used to view all ratings for a given course.
        /// It supports pagination and optional filtering by the rating value (e.g., only show 5-star ratings).
        /// </remarks>
        /// <param name="courseId">The ID of the course to retrieve ratings for.</param>
        /// <param name="query">The pagination and filtering parameters (pageNumber, pageSize, ratingFilter).</param>
        /// <returns>A paginated list of course ratings.</returns>
        [HttpGet("course/{courseId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<CourseRatingResponse>>> GetRatingsForCourse(
            int courseId,
            [FromQuery] GetCourseRatingsQuery query)
        {
            query.CourseId = courseId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a specific course rating.
        /// </summary>
        /// <remarks>
        /// This action can only be performed by the user who created the rating or by an administrator.
        /// Access is restricted to authenticated users.
        /// </remarks>
        /// <param name="ratingId">The ID of the rating to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{ratingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new DeleteCourseRatingCommand
            {
                RatingId = ratingId,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent(); // Standard response for a successful DELETE
        }

        /// <summary>
        /// Adds a new rating and optional review to a course.
        /// </summary>
        /// <remarks>
        /// A user must be authenticated and enrolled in the course to add a rating.
        /// A user cannot rate the same course more than once.
        /// </remarks>
        /// <param name="request">The request containing the course ID, rating value, and review text.</param>
        /// <returns>The ID of the newly created course rating.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRating([FromBody] AddCourseRatingRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var command = new AddCourseRatingCommand
            {
                CourseId = request.CourseId,
                UserId = userId,
                RatingValue = request.RatingValue,
                ReviewText = request.ReviewText
            };

            var ratingId = await _mediator.Send(command);

            return Ok(new { CourseRatingId = ratingId });
        }
    }
} 