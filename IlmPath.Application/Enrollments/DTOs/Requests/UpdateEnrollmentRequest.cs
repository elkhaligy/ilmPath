using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Enrollments.DTOs.Requests;
public class UpdateEnrollmentRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; }
}
