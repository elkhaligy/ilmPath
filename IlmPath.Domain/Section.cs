using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class Section
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CourseId { get; set; } // FK to Course
    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }

    // Navigation property
    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
