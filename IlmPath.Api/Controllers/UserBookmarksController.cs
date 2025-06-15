using AutoMapper;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Invoices.Commands.CreateInvoice;
using IlmPath.Application.Invoices.Commands.Delete_Invoice;
using IlmPath.Application.Invoices.Commands.UpdateInvoice;
using IlmPath.Application.Invoices.DTOs.Requests;
using IlmPath.Application.Invoices.DTOs.Responses;
using IlmPath.Application.Invoices.Queries.GetAllInvoices;
using IlmPath.Application.Invoices.Queries.GetInvoiceById;
using IlmPath.Application.UserBookmarks.Commands.CreateUserBookmark;
using IlmPath.Application.UserBookmarks.Commands.DeleteUserBookmark;
using IlmPath.Application.UserBookmarks.Commands.UpdateUserBookmark;
using IlmPath.Application.UserBookmarks.DTOs.Requests;
using IlmPath.Application.UserBookmarks.DTOs.Responses;
using IlmPath.Application.UserBookmarks.Queries.GetAllUserBookmarks;
using IlmPath.Application.UserBookmarks.Queries.GetUserBookmarkById;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserBookmarksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public UserBookmarksController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    // GET: api/userBookmarks
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserBookmarkResponse>>> GetAll([FromQuery] GetAllUserBookmarksQuery query)
    {
        var (userBookmarks, totalCount) = await _mediator.Send(query);
        var userBookmarksResponses = _mapper.Map<List<UserBookmarkResponse>>(userBookmarks);

        return Ok(new PagedResult<UserBookmarkResponse>(userBookmarksResponses, totalCount, query.PageNumber, query.PageSize));
    }

    // GET: api/userBookmark/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserBookmarkResponse>> GetById(int id)
    {
        var query = new GetUserBookmarkByIdQuery(id);
        var userBookmark = await _mediator.Send(query);

        if (userBookmark == null)
            return NotFound();

        return Ok(_mapper.Map<UserBookmarkResponse>(userBookmark));
    }


    // POST: api/userBookmark
    [HttpPost]
    public async Task<ActionResult<UserBookmarkResponse>> Create(CreateUserBookmarkRequest request)
    {
        var command = _mapper.Map<CreateUserBookmarkCommand>(request);

        var userBookmark = await _mediator.Send(command);
        var userBookmarkResponse = _mapper.Map<UserBookmarkResponse>(userBookmark);

        return CreatedAtAction(nameof(GetById), new { id = userBookmarkResponse.Id }, userBookmarkResponse);
    }


    // PUT: api/userBookmark/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<UserBookmarkResponse>> Update(int id, UpdateUserBookmarkRequest request)
    {
        var command = _mapper.Map<UpdateUserBookmarkCommand>((request, id));

        var userBookmark = await _mediator.Send(command);

        if (userBookmark == null)
            return NotFound();

        return Ok(_mapper.Map<UserBookmarkResponse>(userBookmark));
    }

    //DELETE: api/userBookmark/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteUserBookmarkCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
