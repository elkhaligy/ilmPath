using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.DTOs.Requests;

public class UpdateInvoiceItemRequest
{

    [Required]
    public int InvoiceId { get; set; } // FK to Invoice

    [Required]
    public int CourseId { get; set; } // FK to Course

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty; // e.g., "Enrollment in [Course Title]"

    [Column(TypeName = "decimal(18,2)")]
    public decimal OriginalUnitPrice { get; set; } // Price before discount

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAppliedOnItem { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } // Price after discount for this item

}
