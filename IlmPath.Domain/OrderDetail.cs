using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class OrderDetail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid PaymentId { get; set; }
    [ForeignKey("PaymentId")]
    public virtual Payment? Payment { get; set; }

    [Required]
    public Guid EnrollmentId { get; set; }
    [ForeignKey("EnrollmentId")]
    public virtual Enrollment? Enrollment { get; set; }

    // Denormalized for easier querying, but EnrollmentId is the true link to the purchased item.
    // Could also directly link CourseId if preferred, ensure consistency.
    [Required]
    public Guid CourseId { get; set; }
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }


    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtPurchase { get; set; }
}