using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentByIdAsync(int id);
    Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
    Task<(IEnumerable<Payment> payments, int TotalCount)> GetAllPaymentsAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId);
    Task AddPaymentAsync(Payment payment);
    Task UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int id);
} 