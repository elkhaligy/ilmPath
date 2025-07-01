using IlmPath.Application.Carts.DTOs.Response;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.Queries
{
    public record GetCartByUserIdQuery(string UserId) : IRequest<CartResponse?>;

}
