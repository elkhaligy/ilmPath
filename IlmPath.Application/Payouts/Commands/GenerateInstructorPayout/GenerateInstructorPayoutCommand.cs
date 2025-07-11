using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Payouts.Commands.GenerateInstructorPayout;

public class GenerateInstructorPayoutCommand : IRequest<InstructorPayout>
{
    public string InstructorId { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
} 