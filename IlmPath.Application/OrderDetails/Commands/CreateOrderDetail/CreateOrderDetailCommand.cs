using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Commands.CreateOrderDetail;

public record CreateOrderDetailCommand(int PaymentId, int EnrollmentId, int CourseId, decimal PriceAtPurchase): IRequest<OrderDetail>;
