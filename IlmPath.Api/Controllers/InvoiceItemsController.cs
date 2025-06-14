using AutoMapper;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.InvoiceItems.Commands.CreateInvoiceItem;
using IlmPath.Application.InvoiceItems.Commands.DeleteInvoiceItem;
using IlmPath.Application.InvoiceItems.Commands.UpdateInvoiceItem;
using IlmPath.Application.InvoiceItems.DTOs.Requests;
using IlmPath.Application.InvoiceItems.DTOs.Responses;
using IlmPath.Application.InvoiceItems.Queries.GetAllInvoiceItems;
using IlmPath.Application.InvoiceItems.Queries.GetInvoiceItemById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public InvoiceItemsController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    // GET: api/invoiceItems
    [HttpGet]
    public async Task<ActionResult<PagedResult<InvoiceItemResponse>>> GetAll([FromQuery] GetAllInvoiceItemsQuery query)
    {
        var (invoiceItems, totalCount) = await _mediator.Send(query);
        var invoiceItemResponses = _mapper.Map<List<InvoiceItemResponse>>(invoiceItems);

        return Ok(new PagedResult<InvoiceItemResponse>(invoiceItemResponses, totalCount, query.PageNumber, query.PageSize));
    }

    // GET: api/invoiceItems/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceItemResponse>> GetById(int id)
    {
        var query = new GetInvoiceItemByIdQuery(id);
        var invoiceItem = await _mediator.Send(query);

        if (invoiceItem == null)
            return NotFound();

        return Ok(_mapper.Map<InvoiceItemResponse>(invoiceItem));
    }


    // POST: api/invoiceItems
    [HttpPost]
    public async Task<ActionResult<InvoiceItemResponse>> Create(CreateInvoiceItemRequest request)
    {
        var command = _mapper.Map<CreateInvoiceItemCommand>(request);

        var invoiceItem = await _mediator.Send(command);
        var invoiceItemResponse = _mapper.Map<InvoiceItemResponse>(invoiceItem);

        return CreatedAtAction(nameof(GetById), new { id = invoiceItemResponse.Id }, invoiceItemResponse);
    }


    // PUT: api/invoiceItems/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceItemResponse>> Update(int id, UpdateInvoiceItemRequest request)
    {
        var command = _mapper.Map<UpdateInvoiceItemCommand>((request, id));

        var invoiceItem = await _mediator.Send(command);

        if (invoiceItem == null)
            return NotFound();

        return Ok(_mapper.Map<InvoiceItemResponse>(invoiceItem));
    }

    //DELETE: api/invoiceItems/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteInvoiceItemCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}

