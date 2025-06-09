using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Payment
{
    [Key]
    public int Id { get; set; }  // Or string if payment gateway ID is preferred as PK

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? User { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // e.g., "Credit Card", "PayPal"

    [StringLength(255)]
    public string? TransactionId { get; set; } // From payment gateway

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty; // e.g., "Completed", "Pending", "Failed"

    // Navigation properties
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<AppliedCoupon> AppliedCoupons { get; set; } = new List<AppliedCoupon>();
}