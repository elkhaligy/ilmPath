using MediatR;

namespace IlmPath.Application.Payments.Commands.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(
    string UserId,
    string SuccessUrl,
    string CancelUrl
) : IRequest<(string, string)>; // Returns the checkout URL and session ID