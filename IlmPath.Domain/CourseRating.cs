using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class CourseRating
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CourseId { get; set; } // FK to Course
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    [Required]
    public string StudentId { get; set; } = string.Empty; // FK to ApplicationUser
    [ForeignKey("StudentId")]
    public virtual ApplicationUser? Student { get; set; }

    [Range(1, 5)]
    public int RatingValue { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}