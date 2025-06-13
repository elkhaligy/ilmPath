using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IlmPath.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ProfileImageUrl { get; set; } 

    // Navigation properties
    public virtual Cart? Cart { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<CourseRating> CourseRatings { get; set; } = new List<CourseRating>();
    public virtual ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
    public virtual ICollection<UserBookmark> Bookmarks { get; set; } = new List<UserBookmark>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<AppliedCoupon> AppliedCoupons { get; set; } = new List<AppliedCoupon>();
}
