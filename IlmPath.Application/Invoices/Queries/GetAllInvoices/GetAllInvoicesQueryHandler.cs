using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Enrollments.Queries.GetAllEnrollments;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Queries.GetAllInvoices;
 
public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, (IEnumerable<Invoice>, int count)>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetAllInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<(IEnumerable<Invoice>, int count)> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var (invoices, totalCount) = await _invoiceRepository.GetAllInvoicesAsync(request.PageNumber, request.PageSize);
        return (invoices, totalCount);
    }
}