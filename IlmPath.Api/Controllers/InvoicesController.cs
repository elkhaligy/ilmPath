using AutoMapper;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Invoices.Commands.CreateInvoice;
using IlmPath.Application.Invoices.Commands.Delete_Invoice;
using IlmPath.Application.Invoices.Commands.SendInvoiceEmail;
using IlmPath.Application.Invoices.Commands.UpdateInvoice;
using IlmPath.Application.Invoices.DTOs.Requests;
using IlmPath.Application.Invoices.DTOs.Responses;
using IlmPath.Application.Invoices.Queries.GetAllInvoices;
using IlmPath.Application.Invoices.Queries.GetInvoiceById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public InvoicesController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    // GET: api/invoices
    [HttpGet]
    public async Task<ActionResult<PagedResult<InvoiceResponse>>> GetAll([FromQuery] GetAllInvoicesQuery query)
    {
        var (invoices, totalCount) = await _mediator.Send(query);
        var invoiceResponses = _mapper.Map<List<InvoiceResponse>>(invoices);

        return Ok(new PagedResult<InvoiceResponse>(invoiceResponses, totalCount, query.PageNumber, query.PageSize));
    }

    // GET: api/invoices/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceResponse>> GetById(int id)
    {
        var query = new GetInvoiceByIdQuery(id);
        var invoice = await _mediator.Send(query);

        if (invoice == null)
            return NotFound();

        return Ok(_mapper.Map<InvoiceResponse>(invoice));
    }


    // POST: api/invoices
    [HttpPost]
    public async Task<ActionResult<InvoiceResponse>> Create(CreateInvoiceRequest request)
    {
        var command = _mapper.Map<CreateInvoiceCommand>(request);

        var invoice = await _mediator.Send(command);
        var invoiceResponse = _mapper.Map<InvoiceResponse>(invoice);

        return CreatedAtAction(nameof(GetById), new { id = invoiceResponse.Id }, invoiceResponse);
    }

    // POST: api/invoices/{id}/send-test-email
    [HttpPost("{id}/send-test-email")]
    public async Task<IActionResult> SendTestEmail(int id)
    {
        var command = new SendInvoiceEmailCommand { InvoiceId = id };
        await _mediator.Send(command);
        return Ok(new { message = "Test email sent successfully." });
    }


    // PUT: api/invoices/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceResponse>> Update(int id, UpdateInvoiceRequest request)
    {
        var command = _mapper.Map<UpdateInvoiceCommand>((request, id));

        var invoice = await _mediator.Send(command);

        if (invoice == null)
            return NotFound();

        return Ok(_mapper.Map<InvoiceResponse>(invoice));
    }

    //DELETE: api/invoices/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteInvoiceCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
