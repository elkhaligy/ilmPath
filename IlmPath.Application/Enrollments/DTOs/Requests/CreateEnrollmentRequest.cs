using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.DTOs.Requests;

public class CreateEnrollmentRequest
{
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser

    [Required]
    public int CourseId { get; set; } // FK to Course

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; }
}
