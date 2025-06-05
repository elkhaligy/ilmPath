using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class InvoiceItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InvoiceId { get; set; } // FK to Invoice
    [ForeignKey("InvoiceId")]
    public virtual Invoice? Invoice { get; set; }

    [Required]
    public Guid CourseId { get; set; } // FK to Course
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty; // e.g., "Enrollment in [Course Title]"
    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } // Price after discount for this item

    [Column(TypeName = "decimal(18,2)")]
    public decimal OriginalUnitPrice { get; set; } // Price before discount

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAppliedOnItem { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; } // Final UnitPrice * Quantity
}