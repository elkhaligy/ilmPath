using IlmPath.Application.Lectures.Commands.CreateLecture;
using IlmPath.Application.Lectures.DTOs;
using IlmPath.Application.Lectures.DTOs.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using IlmPath.Application.Lectures.Queries.GetLectureById;
using IlmPath.Application.Lectures.Queries.GetLecturesBySection;
using System.Collections.Generic;
using IlmPath.Application.Lectures.Commands.UpdateLecture;
using IlmPath.Application.Lectures.Commands.DeleteLecture;
using IlmPath.Application.Lectures.Commands.UpdateLectureOrder;
using IlmPath.Application.Lectures.Commands.ToggleLecturePreview;
using IlmPath.Application.Lectures.Commands.UploadVideo;

namespace IlmPath.Api.Controllers;

// Endpoints for Lectures
// POST /api/sections/{sectionId}/lectures
// GET /api/lectures/{id}
// PUT /api/lectures/{id}
// DELETE /api/lectures/{id}
// GET /api/sections/{sectionId}/lectures
// POST /api/lectures/{id}/video

[Route("api/")]
[ApiController]
public class LecturesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LecturesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("sections/{sectionId}/lectures")]
    [ProducesResponseType(typeof(LectureResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(int sectionId, [FromBody] CreateLectureRequest request)
    {
        var command = _mapper.Map<CreateLectureCommand>(request);
        command.SectionId = sectionId;

        var lectureResponse = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = lectureResponse.Id }, lectureResponse);
    }

    [HttpPost("lectures/{id}/video")]
    [ProducesResponseType(typeof(UploadVideoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadVideo(int id, IFormFile videoFile)
    {
        if (videoFile == null || videoFile.Length == 0)
        {
            return BadRequest("No video file provided");
        }

        var command = new UploadVideoCommand
        {
            LectureId = id,
            VideoFile = videoFile
        };

        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPut("lectures/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLectureRequest request)
    {
        var command = _mapper.Map<UpdateLectureCommand>(request);
        command.Id = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("lectures/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteLectureCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("lectures/{id}/order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateLectureOrderRequest request)
    {
        var command = _mapper.Map<UpdateLectureOrderCommand>((request,id));

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("lectures/{id}/preview")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TogglePreview(int id, [FromBody] ToggleLecturePreviewRequest request)
    {
        var command = new ToggleLecturePreviewCommand
        {
            Id = id,
            IsPreviewAllowed = request.IsPreviewAllowed
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("lectures/{id}", Name = "GetLectureById")]
    [ProducesResponseType(typeof(LectureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LectureResponse>> GetById(int id)
    {
        var query = new GetLectureByIdQuery(id);
        var lecture = await _mediator.Send(query);

        return Ok(lecture);
    }

    [HttpGet("sections/{sectionId}/lectures")]
    [ProducesResponseType(typeof(IEnumerable<LectureResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LectureResponse>>> GetLecturesBySection(int sectionId)
    {
        var query = new GetLecturesBySectionQuery(sectionId);
        var lectures = await _mediator.Send(query);
        return Ok(lectures);
    }
}
