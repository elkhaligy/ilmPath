using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Courses.Commands.CreateCourse;
using IlmPath.Application.Courses.Commands.DeleteCourse;
using IlmPath.Application.Courses.Commands.UpdateCourse;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Application.Courses.Queries.GetAllCourses;
using IlmPath.Application.Courses.Queries.GetCourseById;
using IlmPath.Application.Courses.Queries.GetCourseWithContent;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(IMediator _mediator) : ControllerBase
    {
        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CourseResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CourseResponse>>> GetAll([FromQuery] GetAllCoursesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseResponse>> GetById(int id)
        {
            var query = new GetCourseByIdQuery(id);
            var course = await _mediator.Send(query);

          
            return course != null ? Ok(course) : NotFound();
        }

        [HttpGet("{id:int}/learn")]
        [ProducesResponseType(typeof(CourseWithContentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseWithContentResponse>> GetCourseWithContent(int id)
        {
            var query = new GetCourseWithContentQuery(id);
            var courseWithContent = await _mediator.Send(query);
            return Ok(courseWithContent);
        }

        [HttpPost]
        [Authorize] // ✅ Require authentication
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromForm] CreateCourseRequest request)
        {
            var instructorId = GetCurrentUserId(); // ✅ Extract from auth token
            if (string.IsNullOrEmpty(instructorId))
            {
                return Unauthorized("User not authenticated");
            }

            var command = new CreateCourseCommand(
                request.Title, 
                request.Description, 
                request.Price, 
                instructorId, // ✅ Use authenticated user's ID
                request.CategoryId, 
                request.ThumbnailFile,
                request.IsPublished);
            var courseResponse = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = courseResponse.Id }, courseResponse);
        }



        [HttpPut("{id:int}")]
        [Authorize] // ✅ Require authentication for updates
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCourseRequest request)
        {
            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User not authenticated");
            }

            // TODO: Add authorization check to ensure user owns this course
            // For now, we'll let the command handler deal with authorization

            var command = new UpdateCourseCommand(
                id, 
                request.Title, 
                request.Description, 
                request.Price, 
                request.IsPublished, 
                request.ThumbnailImageUrl, 
                request.CategoryId, 
                request.ThumbnailFile);
            await _mediator.Send(command);

            return NoContent();
        }


        [HttpGet("category")]
        [ProducesResponseType(typeof(PagedResult<CourseResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CourseResponse>>> GetCoursesByCategory([FromQuery]GetCoursesByCategoryIdQuery query)
        {
            var mediatRQuery = new GetCoursesByCategoryIdQuery(query.CategoryId, query.PageNumber, query.PageSize);
            var result = await _mediator.Send(mediatRQuery);
            return Ok(result);
        }

        [HttpGet("instructor")]
        [Authorize] // ✅ Also secure this endpoint
        [ProducesResponseType(typeof(PagedResult<CourseResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CourseResponse>>> GetCoursesByInstructor([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var instructorId = GetCurrentUserId(); // ✅ Extract from auth token
            if (string.IsNullOrEmpty(instructorId))
            {
                return Unauthorized("User not authenticated");
            }

            var mediatRQuery = new GetCoursesByInstructorIdQuery(instructorId, pageNumber, pageSize);
            var result = await _mediator.Send(mediatRQuery);
            return Ok(result);
        }



        [HttpDelete("{id:int}")]
        [Authorize] // ✅ Require authentication for deletes
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User not authenticated");
            }

            // TODO: Add authorization check to ensure user owns this course
            // For now, we'll let the command handler deal with authorization

            var command = new DeleteCourseCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }




    }
}
