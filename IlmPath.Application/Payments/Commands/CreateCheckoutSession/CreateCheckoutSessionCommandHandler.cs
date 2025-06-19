using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Exceptions;
using MediatR;

namespace IlmPath.Application.Payments.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, (string, string)>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStripeService _stripeService;

    public CreateCheckoutSessionCommandHandler(
        ICartRepository cartRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        IStripeService stripeService)
    {
        _cartRepository = cartRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _stripeService = stripeService;
    }

    public async Task<(string, string)> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        // 1. Get user's cart
        var cart = await _cartRepository.GetCartAsync(request.UserId);
        if (cart == null || !cart.Items.Any())
        {
            throw new NotFoundException("Cart is empty or not found");
        }

        // 2. Validate all courses exist and user is not already enrolled
        foreach (var cartItem in cart.Items)
        {
            // Check if course exists
            var course = await _courseRepository.GetByIdAsync(cartItem.CourseId);
            if (course == null)
            {
                throw new NotFoundException($"Course with ID {cartItem.CourseId} not found");
            }

            // Check if user is already enrolled (get all enrollments and check)
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync(1, 1000); // Get all enrollments
            var userEnrollments = enrollments.enrollments.Where(e => e.UserId == request.UserId);
            var isAlreadyEnrolled = userEnrollments.Any(e => e.CourseId == cartItem.CourseId);
            
            if (isAlreadyEnrolled)
            {
                throw new InvalidOperationException($"User is already enrolled in course: {course.Title}");
            }
        }

        // 3. Create checkout session with cart items
        var (checkoutUrl, sessionId) = await _stripeService.CreateCheckoutSessionAsync(
            cart.Items.ToList(),
            request.UserId,
            request.SuccessUrl,
            request.CancelUrl);

        return (checkoutUrl, sessionId);
    }
} 