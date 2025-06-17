using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Commands.DeleteOrderDetail;

public record DeleteOrderDetailCommand(int id) : IRequest<bool>;

