using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.Commands
{
    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, Cart>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveItemFromCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.UserId) ?? new Cart(request.UserId);

            var itemToRemove = cart.Items.FirstOrDefault(item => item.CourseId == request.CourseId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
            }
            else 
            {
                throw new NotFoundException("No Items to remove",request.CourseId);
            }

                return await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
