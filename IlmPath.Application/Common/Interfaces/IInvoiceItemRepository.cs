using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces;

public interface IInvoiceItemRepository
{
    Task<InvoiceItem?> GetInvoiceItemByIdAsync(int id);
    Task<(IEnumerable<InvoiceItem> invoiceItems, int TotalCount)> GetAllInvoiceItemsAsync(int pageNumber, int pageSize);
    Task AddInvoiceItemAsync(InvoiceItem invoiceItem);
    Task UpdateInvoiceItemAsync(InvoiceItem invoiceItem);
    Task DeleteInvoiceItemAsync(int id);
}
