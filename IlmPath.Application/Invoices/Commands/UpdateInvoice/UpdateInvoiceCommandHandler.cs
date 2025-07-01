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

namespace IlmPath.Application.Invoices.Commands.UpdateInvoice;
public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Invoice>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    public UpdateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    public async Task<Invoice> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.Id);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        invoice = _mapper.Map<Invoice>(request);

        await _invoiceRepository.UpdateInvoiceAsync(invoice);

        return invoice;
    }
}
