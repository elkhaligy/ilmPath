using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.DTOs.Responses;

public class InvoiceResponse
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; }
    public string UserId { get; set; }
    public int PaymentId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? BillingAddress { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }
}
