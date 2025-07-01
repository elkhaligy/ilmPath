using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.DTOs.Requests;

public class CreateOrderDetailRequest
{

    [Required]
    public int PaymentId { get; set; }

    [Required]
    public int EnrollmentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtPurchase { get; set; }
}
