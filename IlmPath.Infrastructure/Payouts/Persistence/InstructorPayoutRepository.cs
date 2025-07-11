using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IlmPath.Infrastructure.Payouts.Persistence;

public class InstructorPayoutRepository : IInstructorPayoutRepository
{
    private readonly ApplicationDbContext _context;

    public InstructorPayoutRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InstructorPayout?> GetPayoutByIdAsync(int id)
    {
        return await _context.InstructorPayouts
            .Include(p => p.Instructor)
            .Include(p => p.PayoutEnrollments)
                .ThenInclude(pe => pe.Enrollment)
                    .ThenInclude(e => e.User)
            .Include(p => p.PayoutEnrollments)
                .ThenInclude(pe => pe.Enrollment)
                    .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(IEnumerable<InstructorPayout> payouts, int TotalCount)> GetAllPayoutsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.InstructorPayouts.CountAsync();

        var payouts = await _context.InstructorPayouts
            .Include(p => p.Instructor)
            .Include(p => p.PayoutEnrollments)
            .OrderByDescending(p => p.PayoutDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (payouts, totalCount);
    }

    public async Task<(IEnumerable<InstructorPayout> payouts, int TotalCount)> GetPayoutsByInstructorIdAsync(string instructorId, int pageNumber, int pageSize)
    {
        var query = _context.InstructorPayouts
            .Where(p => p.InstructorId == instructorId)
            .Include(p => p.Instructor)
            .Include(p => p.PayoutEnrollments);

        var totalCount = await query.CountAsync();

        var payouts = await query
            .OrderByDescending(p => p.PayoutDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (payouts, totalCount);
    }

    public async Task<InstructorPayout> AddPayoutAsync(InstructorPayout payout)
    {
        await _context.InstructorPayouts.AddAsync(payout);
        await _context.SaveChangesAsync();
        
        // Return the payout with generated ID
        return await GetPayoutByIdAsync(payout.Id) ?? payout;
    }

    public async Task UpdatePayoutAsync(InstructorPayout payout)
    {
        var existingPayout = await _context.InstructorPayouts.FindAsync(payout.Id);
        if (existingPayout == null)
            throw new NotFoundException(nameof(InstructorPayout), payout.Id);

        existingPayout.Status = payout.Status;
        existingPayout.PaymentMethod = payout.PaymentMethod;
        existingPayout.ExternalTransactionId = payout.ExternalTransactionId;
        existingPayout.Notes = payout.Notes;
        existingPayout.ProcessedDate = payout.ProcessedDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePayoutAsync(int id)
    {
        var payout = await _context.InstructorPayouts.FindAsync(id);
        if (payout == null)
            throw new NotFoundException(nameof(InstructorPayout), id);

        _context.InstructorPayouts.Remove(payout);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetUnpaidEnrollmentsAsync(string instructorId)
    {
        // Get all enrollments for instructor's courses
        var allEnrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.User)
            .Where(e => e.Course!.InstructorId == instructorId)
            .ToListAsync();

        // Get all enrollments that have already been paid
        var paidEnrollmentIds = await _context.PayoutEnrollments
            .Where(pe => pe.Payout!.InstructorId == instructorId && pe.Payout.Status != "Rejected")
            .Select(pe => pe.EnrollmentId)
            .ToListAsync();

        // Return enrollments that haven't been paid yet
        return allEnrollments.Where(e => !paidEnrollmentIds.Contains(e.Id));
    }

    public async Task<bool> IsEnrollmentPaidAsync(int enrollmentId)
    {
        return await _context.PayoutEnrollments
            .AnyAsync(pe => pe.EnrollmentId == enrollmentId && pe.Payout!.Status != "Rejected");
    }

    public async Task<decimal> GetPendingBalanceAsync(string instructorId)
    {
        var unpaidEnrollments = await GetUnpaidEnrollmentsAsync(instructorId);
        return unpaidEnrollments.Sum(e => e.PricePaid);
    }
} 