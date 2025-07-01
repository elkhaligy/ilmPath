using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Commands.CreateInvoiceItem;
 
public class CreateInvoiceItemCommandHandler : IRequestHandler<CreateInvoiceItemCommand, InvoiceItem>
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    private readonly IMapper _mapper;

    public CreateInvoiceItemCommandHandler(IInvoiceItemRepository invoiceItemRepository, IMapper mapper)
    {
        _invoiceItemRepository = invoiceItemRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceItem> Handle(CreateInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        // Create invoiceItem
        var invoiceItem = _mapper.Map<InvoiceItem>(request);

        // Add it to the db
        await _invoiceItemRepository.AddInvoiceItemAsync(invoiceItem);
        
        // Return invoiceItem
        return invoiceItem;
    }
}
