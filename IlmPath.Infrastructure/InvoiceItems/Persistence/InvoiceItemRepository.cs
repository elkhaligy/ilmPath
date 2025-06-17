using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.InvoiceItems.Persistence;

public class InvoiceItemRepository : IInvoiceItemRepository
{
    private readonly ApplicationDbContext _context;


    public InvoiceItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddInvoiceItemAsync(InvoiceItem invoiceItem)
    {
        await _context.InvoiceItems.AddAsync(invoiceItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteInvoiceItemAsync(int id)
    {
        var invoiceItem = await _context.InvoiceItems.FindAsync(id);
        if (invoiceItem == null)
            throw new NotFoundException(nameof(InvoiceItem), id);

        _context.InvoiceItems.Remove(invoiceItem);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<InvoiceItem> invoiceItems, int TotalCount)> GetAllInvoiceItemsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.InvoiceItems.CountAsync();

        var invoiceItems = await _context.InvoiceItems
        .Include(i => i.Invoice)
        .Include(i => i.Course)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (invoiceItems, totalCount);
    }

    public async Task<InvoiceItem?> GetInvoiceItemByIdAsync(int id)
    {
        return await _context.InvoiceItems
        .Include(i => i.Invoice)
        .Include(i => i.Course)
        .FirstOrDefaultAsync(i=>i.Id==id);
    }

    public async Task UpdateInvoiceItemAsync(InvoiceItem invoiceItem)
    {
        var existingInvoiceItem = await _context.InvoiceItems.FindAsync(invoiceItem.Id);
        if (existingInvoiceItem == null)
            throw new NotFoundException(nameof(InvoiceItem), invoiceItem.Id);

        existingInvoiceItem.InvoiceId = invoiceItem.InvoiceId;
        existingInvoiceItem.CourseId = invoiceItem.CourseId;
        existingInvoiceItem.Description = invoiceItem.Description;
        existingInvoiceItem.OriginalUnitPrice = invoiceItem.UnitPrice;
        existingInvoiceItem.DiscountAppliedOnItem = invoiceItem.DiscountAppliedOnItem;
        existingInvoiceItem.UnitPrice = invoiceItem.UnitPrice;

        await _context.SaveChangesAsync();
    }
}
