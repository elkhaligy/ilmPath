using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.Commands
{
    public record RemoveItemFromCartCommand(string UserId, int CourseId) : IRequest<Cart>;
   
}
