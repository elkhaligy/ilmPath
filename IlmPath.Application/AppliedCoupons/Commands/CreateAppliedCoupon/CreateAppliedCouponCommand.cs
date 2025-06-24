
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Commands.CreateAppliedCoupon;
public record CreateAppliedCouponCommand(int CouponId, string UserId, int PaymentId, decimal DiscountAmountApplied, DateTime AppliedAt) : IRequest<AppliedCoupon>;
