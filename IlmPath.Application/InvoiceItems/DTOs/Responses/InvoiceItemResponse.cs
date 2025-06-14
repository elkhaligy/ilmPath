using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.DTOs.Responses;

public class InvoiceItemResponse
{
    public int Id { get; set; }
    public int InvoiceId { get; set; } // FK to Invoice
    public int CourseId { get; set; } // FK to Course
    public string Description { get; set; } = string.Empty; // e.g., "Enrollment in [Course Title]"
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; } // Price after discount for this item

    public decimal OriginalUnitPrice { get; set; } // Price before discount

    public decimal DiscountAppliedOnItem { get; set; }
    public decimal LineTotal { get; set; } // Final UnitPrice * Quantity
}
