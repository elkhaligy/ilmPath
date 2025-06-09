using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Course
{
    [Key]
    public int Id { get; set; } 

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string InstructorId { get; set; } = string.Empty; // FK to ApplicationUser
    public virtual ApplicationUser? Instructor { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string? ThumbnailImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = false;

    public int? CategoryId { get; set; } // FK to Category
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    // Navigation properties
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
    public virtual ICollection<CourseRating> Ratings { get; set; } = new List<CourseRating>();
    public virtual ICollection<UserBookmark> BookmarkedByUsers { get; set; } = new List<UserBookmark>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public virtual ICollection<Coupon> ApplicableCoupons { get; set; } = new List<Coupon>(); // Course-specific coupons
}