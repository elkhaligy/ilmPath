using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class AppliedCoupon
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CouponId { get; set; } // FK to Coupon
    public virtual Coupon? Coupon { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int PaymentId { get; set; } // FK to Payment
    public virtual Payment? Payment { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmountApplied { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}