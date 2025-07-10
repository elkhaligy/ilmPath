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

namespace IlmPath.Infrastructure.Invoices.Persistence
{
    class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                throw new NotFoundException(nameof(Invoice), id);

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Invoice> invoices, int TotalCount)> GetAllInvoicesAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Invoices.CountAsync();

            var invoices = await _context.Invoices
            .Include(i => i.Payment)
            .Include(i => i.User)
            .OrderByDescending(i => i.IssueDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (invoices, totalCount);
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                    .Include(i => i.User)
                    .Include(i => i.Payment)
                    .Include(i => i.Items)
                    .ThenInclude(item => item.Course)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            var existingInvoice = await _context.Invoices.FindAsync(invoice.Id);
            if (existingInvoice == null)
                throw new NotFoundException(nameof(Invoice), invoice.Id);
            existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
            existingInvoice.UserId = invoice.UserId;
            existingInvoice.PaymentId = invoice.PaymentId;
            existingInvoice.IssueDate = invoice.IssueDate;
            existingInvoice.DueDate = invoice.DueDate;
            existingInvoice.BillingAddress = invoice.BillingAddress;
            existingInvoice.Status = invoice.Status;
            existingInvoice.Notes = invoice.Notes;


            await _context.SaveChangesAsync();
        }
    }
}
