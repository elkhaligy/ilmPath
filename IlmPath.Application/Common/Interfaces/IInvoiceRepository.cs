using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetInvoiceByIdAsync(int id);
    Task<(IEnumerable<Invoice> invoices, int TotalCount)> GetAllInvoicesAsync(int pageNumber, int pageSize);
    Task AddInvoiceAsync(Invoice invoice);
    Task UpdateInvoiceAsync(Invoice invoice);
    Task DeleteInvoiceAsync(int id);
}
