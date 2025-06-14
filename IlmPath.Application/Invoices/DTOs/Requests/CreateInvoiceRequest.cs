using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.DTOs.Requests;

public class CreateInvoiceRequest
{
    [Required]
    [StringLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty; // Needs a generation strategy

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser

    [Required]
    public int PaymentId { get; set; } // FK to Payment
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public string? BillingAddress { get; set; } // Can be JSON or structured further

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty; // e.g., "Paid", "Pending"
    public string? Notes { get; set; }
}
