using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.DTOs.Responses;

public class OrderDetailResponse
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public int EnrollmentId { get; set; }

    public int CourseId { get; set; }

    public decimal PriceAtPurchase { get; set; }
}
