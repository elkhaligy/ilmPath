using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Enrollment
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int CourseId { get; set; } // FK to Course
    public virtual Course? Course { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; }

    // Navigation property (if linking to OrderDetail directly from here)
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}