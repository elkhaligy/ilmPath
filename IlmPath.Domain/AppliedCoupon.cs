using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class AppliedCoupon
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CouponId { get; set; } // FK to Coupon
    [ForeignKey("CouponId")]
    public virtual Coupon? Coupon { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public Guid PaymentId { get; set; } // FK to Payment
    [ForeignKey("PaymentId")]
    public virtual Payment? Payment { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmountApplied { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}