using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class CartItem
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public int CartId { get; set; } // FK to Cart
    public virtual Cart? Cart { get; set; }

    [Required]
    public int CourseId { get; set; } // FK to Course
    public virtual Course? Course { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}