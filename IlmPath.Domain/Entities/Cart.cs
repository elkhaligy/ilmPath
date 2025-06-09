using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IlmPath.Domain.Entities;

public class Cart
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public string UserId { get; set; } = string.Empty; // FK to ApplicationUser (should be unique)
    public virtual ApplicationUser? User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}