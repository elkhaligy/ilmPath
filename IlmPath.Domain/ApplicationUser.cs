using System.ComponentModel.DataAnnotations;


namespace IlmPath.Domain;

public class ApplicationUser // IdentityUser uses string for Id by default
{
    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public string? ProfilePictureUrl { get; set; }

    // Navigation properties
    public virtual ICollection<Course> InstructedCourses { get; set; } = new List<Course>();
    public virtual ICollection<CourseRating> Ratings { get; set; } = new List<CourseRating>();
    public virtual ICollection<UserBookmark> Bookmarks { get; set; } = new List<UserBookmark>();
    public virtual Cart? Cart { get; set; } // A user has one cart
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<AppliedCoupon> AppliedCoupons { get; set; } = new List<AppliedCoupon>();
    public virtual ICollection<Coupon> CreatedCoupons { get; set; } = new List<Coupon>(); // If admin/instructor creates coupons
}