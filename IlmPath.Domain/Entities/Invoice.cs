using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty; // Needs a generation strategy

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int PaymentId { get; set; } // FK to Payment
    public virtual Payment? Payment { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public string? BillingAddress { get; set; } // Can be JSON or structured further

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty; // e.g., "Paid", "Pending"
    public string? Notes { get; set; }

    // Navigation property
    public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}