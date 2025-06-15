using IlmPath.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IlmPath.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
// Use IdentityDbContext<ApplicationUser> if you don't have a custom ApplicationRole
// The 'string' is for the type of the primary key for Identity tables.
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Courses & Content
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Lecture> Lectures { get; set; }

    // Ratings & Bookmarks
    public DbSet<CourseRating> CourseRatings { get; set; }
    public DbSet<UserBookmark> UserBookmarks { get; set; }

    // Shopping & Payments
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    // Invoicing
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    // Coupons
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<AppliedCoupon> AppliedCoupons { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Important for Identity tables configuration

        // Course
        builder.Entity<Course>(entity =>
        {
            entity.HasOne(c => c.Category)
                  .WithMany(cat => cat.Courses)
                  .HasForeignKey(c => c.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // CourseRating
        builder.Entity<CourseRating>(entity =>
        {
            entity.HasIndex(cr => new { cr.UserId, cr.CourseId })
                  .IsUnique();

            entity.HasOne(cr => cr.User)
                  .WithMany(u => u.CourseRatings)
                  .HasForeignKey(cr => cr.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // UserBookmark
        builder.Entity<UserBookmark>(entity =>
        {
            entity.HasIndex(ub => new { ub.UserId, ub.CourseId })
                  .IsUnique();

            entity.HasOne(ub => ub.User)
                  .WithMany(u => u.Bookmarks)
                  .HasForeignKey(ub => ub.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Cart
        builder.Entity<Cart>(entity =>
        {
            entity.HasIndex(c => c.UserId)
                  .IsUnique();

            entity.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // CartItem
        builder.Entity<CartItem>(entity =>
        {
            entity.HasIndex(ci => new { ci.CartId, ci.CourseId })
                  .IsUnique();

            entity.HasOne(ci => ci.Cart)
                  .WithMany(c => c.Items)
                  .HasForeignKey(ci => ci.CartId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Enrollment
        builder.Entity<Enrollment>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.CourseId })
                  .IsUnique();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Enrollments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Payment
        builder.Entity<Payment>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Payments)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // OrderDetail
        builder.Entity<OrderDetail>(entity =>
        {
            entity.HasOne(od => od.Payment)
                  .WithMany(p => p.OrderDetails)
                  .HasForeignKey(od => od.PaymentId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(od => od.Enrollment)
                  .WithMany(e => e.OrderDetails)
                  .HasForeignKey(od => od.EnrollmentId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(od => od.Course)
                  .WithMany()
                  .HasForeignKey(od => od.CourseId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Invoice
        builder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(i => i.InvoiceNumber)
                  .IsUnique();

            entity.HasOne(i => i.User)
                  .WithMany(u => u.Invoices)
                  .HasForeignKey(i => i.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(i => i.Payment)
                  .WithMany(p => p.Invoices)
                  .HasForeignKey(i => i.PaymentId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Coupon
        builder.Entity<Coupon>(entity =>
        {
            entity.HasIndex(c => c.Code)
                  .IsUnique();

            entity.HasOne(c => c.Course)
                  .WithMany(course => course.ApplicableCoupons)
                  .HasForeignKey(c => c.CourseId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // AppliedCoupon
        builder.Entity<AppliedCoupon>(entity =>
        {
            entity.HasOne(ac => ac.User)
                  .WithMany(u => u.AppliedCoupons)
                  .HasForeignKey(ac => ac.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(ac => ac.Payment)
                  .WithMany(p => p.AppliedCoupons)
                  .HasForeignKey(ac => ac.PaymentId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // Define decimal precision for all relevant properties (Fluent API is more DRY)
        // Alternatively, use [Column(TypeName = "decimal(18,2)")] attribute on each property
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    // Check if already configured by [Column(TypeName = "...")]
                    if (string.IsNullOrEmpty(property.GetColumnType()) && property.GetAnnotations().All(a => a.Name != "Relational:ColumnType"))
                    {
                         property.SetColumnType("decimal(18,2)");
                    }
                }
            }
        }

    }
}