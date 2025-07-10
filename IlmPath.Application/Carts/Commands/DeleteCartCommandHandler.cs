using IlmPath.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.Commands
{
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.DeleteCartAsync(request.UserId);
        }
    }
} 