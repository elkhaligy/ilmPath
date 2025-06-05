using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class UserBookmark
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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}