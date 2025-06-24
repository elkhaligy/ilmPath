using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Commands.DeleteAppliedCoupon;

public record DeleteAppliedCouponCommand(int Id) : IRequest<bool>;

