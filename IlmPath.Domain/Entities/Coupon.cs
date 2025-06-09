using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Coupon
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Required]
    [StringLength(20)]
    public string DiscountType { get; set; } = string.Empty; // "Percentage", "FixedAmount"

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
    public DateTime ValidTo { get; set; }
    public int? MaxUses { get; set; }
    public int CurrentUses { get; set; } = 0;
    public int? MaxUsesPerUser { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinPurchaseAmount { get; set; }

    public int? CourseId { get; set; } // FK to Course, nullable for general coupons
    public virtual Course? Course { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedById { get; set; } // FK to ApplicationUser (Admin/Instructor)
    [ForeignKey("CreatedById")]
    public virtual ApplicationUser? CreatedBy { get; set; }

    // Navigation property
    public virtual ICollection<AppliedCoupon> Usages { get; set; } = new List<AppliedCoupon>();
}