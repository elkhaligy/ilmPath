using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Commands.DeleteInvoiceItem;

public class DeleteInvoiceItemCommandHandler : IRequestHandler<DeleteInvoiceItemCommand, bool>
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;

    public DeleteInvoiceItemCommandHandler(IInvoiceItemRepository invoiceItemRepository)
    {
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<bool> Handle(DeleteInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceItemRepository.DeleteInvoiceItemAsync(request.id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
