using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class PayoutEnrollment
{
    [Key]
    public int Id { get; set; }

    public int PayoutId { get; set; }
    public virtual InstructorPayout? Payout { get; set; }

    public int EnrollmentId { get; set; }
    public virtual Enrollment? Enrollment { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaidToInstructor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 