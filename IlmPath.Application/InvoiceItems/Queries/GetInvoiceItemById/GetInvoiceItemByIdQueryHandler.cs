using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Queries.GetInvoiceItemById;

public class GetInvoiceItemByIdQueryHandler : IRequestHandler<GetInvoiceItemByIdQuery, InvoiceItem>
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;

    public GetInvoiceItemByIdQueryHandler(IInvoiceItemRepository invoiceItemRepository)
    {
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<InvoiceItem> Handle(GetInvoiceItemByIdQuery request, CancellationToken cancellationToken)
    {
        var invoiceItem = await _invoiceItemRepository.GetInvoiceItemByIdAsync(request.Id);

        if (invoiceItem == null)
        {
            throw new NotFoundException(nameof(InvoiceItem), request.Id);
        }

        return invoiceItem;
    }
}

