using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class UserBookmark
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int CourseId { get; set; } // FK to Course
    public virtual Course? Course { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}