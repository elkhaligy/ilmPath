using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Courses.Commands.CreateCourse;
using IlmPath.Application.Courses.Commands.DeleteCourse;
using IlmPath.Application.Courses.Commands.UpdateCourse;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Application.Courses.Queries.GetAllCourses;
using IlmPath.Application.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(IMediator _mediator) : ControllerBase
    {

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

        [HttpPost]
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] decimal price,
            [FromForm] string instructorId,
            [FromForm] int? categoryId,
            [FromForm] IFormFile? thumbnailFile = null)
        {
            var command = new CreateCourseCommand(title, description, price, instructorId, categoryId, thumbnailFile);
            var courseResponse = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = courseResponse.Id }, courseResponse);
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] string title,
            [FromForm] string description,
            [FromForm] decimal price,
            [FromForm] bool isPublished,
            [FromForm] string? thumbnailImageUrl = null,
            [FromForm] int? categoryId = null,
            [FromForm] IFormFile? thumbnailFile = null)
        {
            var command = new UpdateCourseCommand(id, title, description, price, isPublished, thumbnailImageUrl, categoryId, thumbnailFile);
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



        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete(int id)
        {
            var query = new DeleteCourseCommand(id);

            await _mediator.Send(query);
            return NoContent();
        }




    }
}
