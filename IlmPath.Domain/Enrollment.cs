using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class Enrollment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public Guid CourseId { get; set; } // FK to Course
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; }

    // Navigation property (if linking to OrderDetail directly from here)
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}