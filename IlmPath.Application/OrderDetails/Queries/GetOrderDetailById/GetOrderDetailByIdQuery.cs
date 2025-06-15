using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Queries.GetOrderDetailById;

public record GetOrderDetailByIdQuery (int Id) : IRequest<OrderDetail>;
