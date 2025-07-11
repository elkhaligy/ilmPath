using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class InstructorPayout
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string InstructorId { get; set; } = string.Empty;
    public virtual ApplicationUser? Instructor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossAmount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal CommissionRate { get; set; } = 0.30m; // Default 30% commission

    [Column(TypeName = "decimal(18,2)")]
    public decimal CommissionAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetAmount { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed

    [StringLength(100)]
    public string? PaymentMethod { get; set; }

    [StringLength(200)]
    public string? ExternalTransactionId { get; set; }

    public DateTime PayoutDate { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedDate { get; set; }

    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<PayoutEnrollment> PayoutEnrollments { get; set; } = new List<PayoutEnrollment>();
} 