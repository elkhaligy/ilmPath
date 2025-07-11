using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Payouts.Commands.GenerateInstructorPayout;

public class GenerateInstructorPayoutCommandHandler : IRequestHandler<GenerateInstructorPayoutCommand, InstructorPayout>
{
    private readonly IInstructorPayoutRepository _payoutRepository;

    public GenerateInstructorPayoutCommandHandler(IInstructorPayoutRepository payoutRepository)
    {
        _payoutRepository = payoutRepository;
    }

    public async Task<InstructorPayout> Handle(GenerateInstructorPayoutCommand request, CancellationToken cancellationToken)
    {
        // Get all unpaid enrollments for this instructor
        var unpaidEnrollments = await _payoutRepository.GetUnpaidEnrollmentsAsync(request.InstructorId);
        
        if (!unpaidEnrollments.Any())
        {
            throw new InvalidOperationException("No unpaid enrollments found for this instructor.");
        }

        // Calculate amounts
        var grossAmount = unpaidEnrollments.Sum(e => e.PricePaid);
        var commissionRate = 0.30m; // 30% commission
        var commissionAmount = grossAmount * commissionRate;
        var netAmount = grossAmount - commissionAmount;

        // Create the payout
        var payout = new InstructorPayout
        {
            InstructorId = request.InstructorId,
            GrossAmount = grossAmount,
            CommissionRate = commissionRate,
            CommissionAmount = commissionAmount,
            NetAmount = netAmount,
            Status = "Pending",
            PaymentMethod = request.PaymentMethod,
            Notes = request.Notes,
            PayoutDate = DateTime.UtcNow
        };

        // Add payout to database
        var createdPayout = await _payoutRepository.AddPayoutAsync(payout);

        // Create PayoutEnrollment records for each enrollment
        foreach (var enrollment in unpaidEnrollments)
        {
            var instructorShare = enrollment.PricePaid * (1 - commissionRate);
            var payoutEnrollment = new PayoutEnrollment
            {
                PayoutId = createdPayout.Id,
                EnrollmentId = enrollment.Id,
                AmountPaidToInstructor = instructorShare
            };
            
            createdPayout.PayoutEnrollments.Add(payoutEnrollment);
        }

        // Update the payout with enrollment relationships
        await _payoutRepository.UpdatePayoutAsync(createdPayout);

        return createdPayout;
    }
} 