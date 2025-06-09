using System;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Domain.Entities;

public class CourseRating
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CourseId { get; set; } // FK to Course
    public virtual Course? Course { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? User { get; set; }

    [Range(1, 5)]
    public int RatingValue { get; set; }
    public string? ReviewText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}