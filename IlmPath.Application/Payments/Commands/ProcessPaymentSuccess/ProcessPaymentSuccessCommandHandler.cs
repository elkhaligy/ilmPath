using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Payments.Commands.ProcessPaymentSuccess;

public class ProcessPaymentSuccessCommandHandler : IRequestHandler<ProcessPaymentSuccessCommand, bool>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IStripeService _stripeService;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICourseRepository _courseRepository;

    public ProcessPaymentSuccessCommandHandler(
        IPaymentRepository paymentRepository,
        IStripeService stripeService,
        IEnrollmentRepository enrollmentRepository,
        ICartRepository cartRepository,
        ICourseRepository courseRepository)
    {
        _paymentRepository = paymentRepository;
        _stripeService = stripeService;
        _enrollmentRepository = enrollmentRepository;
        _cartRepository = cartRepository;
        _courseRepository = courseRepository;
    }

    public async Task<bool> Handle(ProcessPaymentSuccessCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get session details from Stripe
            var sessionDetails = await _stripeService.GetSessionDetailsAsync(request.SessionId);
            if (sessionDetails == null)
            {
                return false;
            }

            // Create payment record
            var payment = new Payment
            {
                UserId = request.UserId,
                Amount = sessionDetails.TotalAmount,
                PaymentMethod = "Stripe",
                TransactionId = sessionDetails.PaymentIntentId,
                Status = "Completed", // Use Status instead of PaymentStatus
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddPaymentAsync(payment);

            // Create enrollments for each course
            if (sessionDetails.CourseIds != null && sessionDetails.CourseIds.Any())
            {
                foreach (var courseId in sessionDetails.CourseIds)
                {
                    var enrollment = new Enrollment
                    {
                        UserId = request.UserId,
                        CourseId = courseId,
                        PricePaid = (await _courseRepository.GetByIdAsync(courseId))?.Price ?? 0,
                        EnrollmentDate = DateTime.UtcNow
                    };

                    await _enrollmentRepository.AddEnrollmentAsync(enrollment);
                }
            }

            // Clear user's cart
            await _cartRepository.DeleteCartAsync(request.UserId);

            return true;
        }
        catch
        {
            return false;
        }
    }
} 