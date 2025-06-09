using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    [Required]
    public int EnrollmentId { get; set; }
    public virtual Enrollment? Enrollment { get; set; }

    [Required]
    public int CourseId { get; set; }
    public virtual Course? Course { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtPurchase { get; set; }
}