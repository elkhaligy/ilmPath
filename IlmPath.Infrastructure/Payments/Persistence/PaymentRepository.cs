using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IlmPath.Infrastructure.Payments.Persistence;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.OrderDetails)
            .ThenInclude(od => od.Course)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        return await _context.Payments
            .Include(p => p.User)
            .Include(p => p.OrderDetails)
            .ThenInclude(od => od.Course)
            .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public async Task<(IEnumerable<Payment> payments, int TotalCount)> GetAllPaymentsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Payments.CountAsync();

        var payments = await _context.Payments
            .Include(p => p.User)
            .OrderByDescending(p => p.PaymentDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (payments, totalCount);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId)
    {
        return await _context.Payments
            .Where(p => p.UserId == userId)
            .Include(p => p.OrderDetails)
            .ThenInclude(od => od.Course)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        var existingPayment = await _context.Payments.FindAsync(payment.Id);
        if (existingPayment == null)
            throw new NotFoundException(nameof(Payment), payment.Id);

        existingPayment.Status = payment.Status;
        existingPayment.TransactionId = payment.TransactionId;
        existingPayment.PaymentMethod = payment.PaymentMethod;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
            throw new NotFoundException(nameof(Payment), id);

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
    }
} 