using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Interfaces;

public interface IInstructorPayoutRepository
{
    Task<InstructorPayout?> GetPayoutByIdAsync(int id);
    Task<(IEnumerable<InstructorPayout> payouts, int TotalCount)> GetAllPayoutsAsync(int pageNumber, int pageSize);
    Task<(IEnumerable<InstructorPayout> payouts, int TotalCount)> GetPayoutsByInstructorIdAsync(string instructorId, int pageNumber, int pageSize);
    Task<InstructorPayout> AddPayoutAsync(InstructorPayout payout);
    Task UpdatePayoutAsync(InstructorPayout payout);
    Task DeletePayoutAsync(int id);
    Task<IEnumerable<Enrollment>> GetUnpaidEnrollmentsAsync(string instructorId);
    Task<bool> IsEnrollmentPaidAsync(int enrollmentId);
    Task<decimal> GetPendingBalanceAsync(string instructorId);
} 