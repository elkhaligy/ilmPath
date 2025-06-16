using IlmPath.Application.Carts.DTOs.Response;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.Queries
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartResponse?>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartResponse?> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            
            var cart= await _cartRepository.GetCartAsync(request.UserId) ?? new Cart(request.UserId);

            return new CartResponse
            {
                UserId = cart.UserId,
                Items = cart.Items
            };
        }
    }
}
