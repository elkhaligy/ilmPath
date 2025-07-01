using AutoMapper;
using IlmPath.Application.Sections.Commands.CreateSection;
using IlmPath.Application.Sections.Commands.DeleteSection;
using IlmPath.Application.Sections.Commands.UpdateSection;
using IlmPath.Application.Sections.DTOs;
using IlmPath.Application.Sections.DTOs.Requests;
using IlmPath.Application.Sections.Queries.GetSectionById;
using IlmPath.Application.Sections.Queries.GetSectionsByCourse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

// Endpoints for Sections
// POST /api/courses/{courseId}/sections
// GET /api/sections/{id}
// PUT /api/sections/{id}
// DELETE /api/sections/{id}
// GET /api/courses/{courseId}/sections

[Route("api/")]
[ApiController]
public class SectionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SectionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("courses/{courseId}/sections")]
    [ProducesResponseType(typeof(SectionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int courseId, [FromBody] CreateSectionRequest request)
    {
        var command = _mapper.Map<CreateSectionCommand>(request);
        command.CourseId = courseId;

        var sectionResponse = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = sectionResponse.Id }, sectionResponse);
    }

    [HttpPut("sections/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSectionRequest request)
    {
        var command = _mapper.Map<UpdateSectionCommand>(request);
        command.Id = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("sections/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteSectionCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("sections/{id}", Name = "GetSectionById")]
    [ProducesResponseType(typeof(SectionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SectionResponse>> GetById(int id)
    {
        var query = new GetSectionByIdQuery(id);
        var section = await _mediator.Send(query);

        return Ok(section);
    }

    [HttpGet("courses/{courseId}/sections")]
    [ProducesResponseType(typeof(IEnumerable<SectionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SectionResponse>>> GetSectionsByCourse(int courseId)
    {
        var query = new GetSectionsByCourseQuery(courseId);
        var sections = await _mediator.Send(query);
        return Ok(sections);
    }
} 