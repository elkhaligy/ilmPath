using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Commands.UpdateAppliedCoupon;

public record UpdateAppliedCouponCommand(int Id,int CouponId, string UserId, int PaymentId, decimal DiscountAmountApplied, DateTime AppliedAt) : IRequest<AppliedCoupon>;
