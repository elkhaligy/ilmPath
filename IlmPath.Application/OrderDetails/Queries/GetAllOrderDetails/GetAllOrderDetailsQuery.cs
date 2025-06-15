using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Queries.GetAllOrderDetails;

public record GetAllOrderDetailsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<(IEnumerable<OrderDetail>, int count)>;

