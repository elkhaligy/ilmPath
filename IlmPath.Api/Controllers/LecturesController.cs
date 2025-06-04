using Microsoft.AspNetCore.Mvc;
using IlmPath.Application.Lectures.Commands.CreateLecture;
using MediatR;
using System;
using System.Threading.Tasks;
using FluentResults;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LecturesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LecturesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    public async Task<IActionResult> CreateLecture(CreateLectureCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
    }
}
