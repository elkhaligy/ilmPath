using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Domain.Entities;
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Slug { get; set; } // For SEO-friendly URLs

    // Navigation property
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}