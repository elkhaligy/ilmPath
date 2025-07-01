using AutoMapper;
using Azure.Core;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Application.Enrollments.Commands.CreateEnrollment;
using IlmPath.Application.Enrollments.Commands.DeleteEnrollment;
using IlmPath.Application.Enrollments.Commands.UpdateEnrollment;
using IlmPath.Application.Enrollments.DTOs.Requests;
using IlmPath.Application.Enrollments.DTOs.Responses;
using IlmPath.Application.Enrollments.Queries.GetAllEnrollments;
using IlmPath.Application.Enrollments.Queries.GetEnrollmentById;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public EnrollmentsController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    // GET: api/enrollments
    [HttpGet]
    public async Task<ActionResult<PagedResult<EnrollmentResponse>>> GetAll([FromQuery] GetAllEnrollmentsQuery query)
    {
        var (enrollments, totalCount) = await _mediator.Send(query);
        var enrollmentResponses = _mapper.Map<List<EnrollmentResponse>>(enrollments);

        return Ok(new PagedResult<EnrollmentResponse>(enrollmentResponses, totalCount, query.PageNumber, query.PageSize));
    }

    // GET: api/enrollments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<EnrollmentResponse>> GetById(int id)
    {
        var query = new GetEnrollmentByIdQuery(id);
        var enrollment = await _mediator.Send(query);

        if (enrollment == null)
            return NotFound();

        return Ok(_mapper.Map<EnrollmentResponse>(enrollment));
    }


    // POST: api/enrollments
    [HttpPost]
    public async Task<ActionResult<EnrollmentResponse>> Create(CreateEnrollmentRequest request)
    {
        var command = _mapper.Map<CreateEnrollmentCommand>(request);

        var enrollment = await _mediator.Send(command);
        var enrollmentResponse = _mapper.Map<EnrollmentResponse>(enrollment);

        return CreatedAtAction(nameof(GetById), new { id = enrollmentResponse.Id }, enrollmentResponse);
    }


    // PUT: api/enrollments/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<EnrollmentResponse>> Update(int id, UpdateEnrollmentRequest request)
    {
        var command = _mapper.Map<UpdateEnrollmentCommand>((request, id));

        var enrollment = await _mediator.Send(command);

        if (enrollment == null)
            return NotFound();

        return Ok(_mapper.Map<EnrollmentResponse>(enrollment));
    }

    // DELETE: api/enrollments/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteEnrollmentCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}

