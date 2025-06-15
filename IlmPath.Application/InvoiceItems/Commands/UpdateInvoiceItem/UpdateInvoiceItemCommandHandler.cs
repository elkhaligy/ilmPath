using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Commands.UpdateInvoiceItem;
 
public class UpdateInvoiceItemCommandHandler : IRequestHandler<UpdateInvoiceItemCommand, InvoiceItem>
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    private readonly IMapper _mapper;


    public UpdateInvoiceItemCommandHandler(IInvoiceItemRepository invoiceItemRepository, IMapper mapper)
    {
        _invoiceItemRepository = invoiceItemRepository;
        _mapper = mapper;
    }

    public async Task<InvoiceItem> Handle(UpdateInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        var invoiceItem = await _invoiceItemRepository.GetInvoiceItemByIdAsync(request.Id);

        if (invoiceItem == null)
        {
            throw new NotFoundException(nameof(InvoiceItem), request.Id);
        }

        invoiceItem = _mapper.Map<InvoiceItem>(request);

        await _invoiceItemRepository.UpdateInvoiceItemAsync(invoiceItem);

        return invoiceItem;
    }
}
