using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Queries.GetInvoiceById;
 
public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, Invoice>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoiceByIdQueryHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<Invoice> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        return invoice;
    }
}

