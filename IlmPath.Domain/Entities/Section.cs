using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Section
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public int CourseId { get; set; } // FK to Course
    public virtual Course? Course { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }

    // Navigation property
    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
