using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
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
    private readonly IMapper _mapper;


    public InvoiceItemRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (invoiceItems, totalCount);
    }

    public async Task<InvoiceItem?> GetInvoiceItemByIdAsync(int id)
    {
        return await _context.InvoiceItems.FindAsync(id);
    }

    public async Task UpdateInvoiceItemAsync(InvoiceItem invoiceItem)
    {
        var existingInvoiceItem = await _context.Invoices.FindAsync(invoiceItem.Id);
        if (existingInvoiceItem == null)
            throw new NotFoundException(nameof(InvoiceItem), invoiceItem.Id);

        _mapper.Map(invoiceItem, existingInvoiceItem);

        await _context.SaveChangesAsync();
    }
}
