using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class CartItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CartId { get; set; } // FK to Cart
    [ForeignKey("CartId")]
    public virtual Cart? Cart { get; set; }

    [Required]
    public Guid CourseId { get; set; } // FK to Course
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}