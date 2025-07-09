using MediatR;

namespace IlmPath.Application.Carts.Commands
{
    public record DeleteCartCommand(string UserId) : IRequest<bool>;
} 