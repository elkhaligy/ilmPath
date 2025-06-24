using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.DTOs.Requests;

public class UpdateAppliedCouponRequest
{
    [Required]
    public int CouponId { get; set; } // FK to Coupon

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser

    [Required]
    public int PaymentId { get; set; } // FK to Payment

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmountApplied { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
