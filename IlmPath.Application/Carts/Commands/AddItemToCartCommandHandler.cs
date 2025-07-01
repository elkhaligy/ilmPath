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
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Cart>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICourseRepository _courseRepository;

        public AddItemToCartCommandHandler(ICartRepository cartRepository, ICourseRepository courseRepository)
        {
            _cartRepository = cartRepository;
            _courseRepository = courseRepository;
        }

        public async Task<Cart> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartAsync(request.UserId) ?? new Cart(request.UserId);

            var itemExists = cart.Items.Any(item => item.CourseId == request.CourseId);
            if (!itemExists)
            {
                var course = await _courseRepository.GetByIdAsync(request.CourseId);
                if (course != null) 
                {
                    cart.Items.Add(new CartItem
                    {
                        CourseId = course.Id,
                        Title = course.Title,
                        Price = course.Price,
                        ThumbnailImageUrl = course.ThumbnailImageUrl
                    });
                }
                else
                {
                    throw new NotFoundException("Invalid Course",request.UserId);
                }
            }

            return await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
