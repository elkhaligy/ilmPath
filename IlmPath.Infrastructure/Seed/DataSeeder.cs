using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Seed;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ICartRepository _cartRepository; 

    public DataSeeder(ApplicationDbContext context
        ,ICartRepository cartRepository
        )
    {
        _context = context;
        _cartRepository = cartRepository;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (await _context.Categories.AnyAsync()) return;

        // Get or create instructor and student users
        var instructorUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync();
        var studentUser = await _context.Users.AsNoTracking().Skip(1).FirstOrDefaultAsync(); // Get second user if exists

        if (instructorUser != null)
        {
            await SeedCategoriesAsync();
            await _context.SaveChangesAsync();

            // Create courses with instructor as the course creator
            await SeedCoursesAsync(instructorUser.Id);
            await _context.SaveChangesAsync();

            await SeedSectionsAsync();
            await _context.SaveChangesAsync();

            await SeedLecturesAsync();
            await _context.SaveChangesAsync();

            await SeedCouponsAsync(instructorUser.Id);
            await _context.SaveChangesAsync();

            // Create cart for student (or instructor if no student exists)
            var cartUserId = studentUser?.Id ?? instructorUser.Id;
            await SeedCartWithItemsAsync(cartUserId);

            // Create enrollments for student user (or instructor if no student exists)
            // This creates a realistic scenario where students enroll in instructor's courses
            var enrollmentUserId = studentUser?.Id ?? instructorUser.Id;
            await SeedEnrollmentsAsync(enrollmentUserId);
            await _context.SaveChangesAsync();

            await SeedCourseRatingsAsync(enrollmentUserId);
            await _context.SaveChangesAsync();

            await SeedUserBookmarksAsync(enrollmentUserId);
            await _context.SaveChangesAsync();

            await SeedPaymentsAsync(enrollmentUserId);
            await _context.SaveChangesAsync();

            await SeedOrderDetailsAsync();
            await _context.SaveChangesAsync();

            await SeedInvoicesAsync(enrollmentUserId);
            await _context.SaveChangesAsync();

            await SeedInvoiceItemsAsync();
            await _context.SaveChangesAsync();

            await SeedAppliedCouponsAsync(enrollmentUserId);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCategoriesAsync()
    {
        if (await _context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { Name = "Web Development", Slug = "web-development" },
            new() { Name = "Data Science", Slug = "data-science" },
            new() { Name = "Mobile Development", Slug = "mobile-development" },
            new() { Name = "Cloud Computing", Slug = "cloud-computing" },
            new() { Name = "Business & Entrepreneurship", Slug = "business-entrepreneurship" }
        };
        await _context.Categories.AddRangeAsync(categories);
    }

    private async Task SeedCoursesAsync(string instructorId)
    {
        if (await _context.Courses.AnyAsync()) return;
        var categories = await _context.Categories.ToListAsync();
        if (!categories.Any()) return;

        var courses = new List<Course>
        {
            new() { Title = "Ultimate ASP.NET Core MVC", Description = "Master the MVC pattern with .NET.", InstructorId = instructorId, Price = 199.99m, IsPublished = true, CategoryId = categories.First(c => c.Slug == "web-development").Id },
            new() { Title = "Python for Data Science A-Z", Description = "Learn Python for data analysis and visualization.", InstructorId = instructorId, Price = 149.99m, IsPublished = true, CategoryId = categories.First(c => c.Slug == "data-science").Id },
            new() { Title = "React Native: Build Mobile Apps", Description = "Develop native apps for iOS and Android.", InstructorId = instructorId, Price = 179.99m, IsPublished = true, CategoryId = categories.First(c => c.Slug == "mobile-development").Id },
            new() { Title = "AWS Certified Solutions Architect", Description = "Prepare for the AWS certification exam.", InstructorId = instructorId, Price = 249.99m, IsPublished = true, CategoryId = categories.First(c => c.Slug == "cloud-computing").Id },
            new() { Title = "Startup Funding Masterclass", Description = "Learn how to secure funding for your startup.", InstructorId = instructorId, Price = 299.99m, IsPublished = false, CategoryId = categories.First(c => c.Slug == "business-entrepreneurship").Id }
        };
        await _context.Courses.AddRangeAsync(courses);
    }

    private async Task SeedSectionsAsync()
    {
        if (await _context.Sections.AnyAsync()) return;
        var courses = await _context.Courses.ToListAsync();
        if (!courses.Any()) return;

        var sections = new List<Section>();
        foreach (var course in courses)
        {
            sections.Add(new Section { Title = $"Introduction to {course.Title}", CourseId = course.Id, Order = 1 });
            sections.Add(new Section { Title = $"Core Concepts of {course.Title}", CourseId = course.Id, Order = 2 });
        }
        await _context.Sections.AddRangeAsync(sections);
    }

    private async Task SeedLecturesAsync()
    {
        if (await _context.Lectures.AnyAsync()) return;
        var sections = await _context.Sections.ToListAsync();
        if (!sections.Any()) return;

        var lectures = new List<Lecture>();
        foreach (var section in sections)
        {
            lectures.Add(new Lecture { Title = "Welcome to the Section", SectionId = section.Id, VideoUrl = "url/to/video/placeholder1", Order = 1, DurationInMinutes = 5, IsPreviewAllowed = true });
            lectures.Add(new Lecture { Title = "First Topic", SectionId = section.Id, VideoUrl = "url/to/video/placeholder2", Order = 2, DurationInMinutes = 15 });
            lectures.Add(new Lecture { Title = "Second Topic", SectionId = section.Id, VideoUrl = "url/to/video/placeholder3", Order = 3, DurationInMinutes = 20 });
        }
        await _context.Lectures.AddRangeAsync(lectures);
    }

    private async Task SeedCouponsAsync(string userId)
    {
        if (await _context.Coupons.AnyAsync()) return;
        var courses = await _context.Courses.ToListAsync();
        if (!courses.Any()) return;

        var coupons = new List<Coupon>
        {
            new() { Code = "SUMMER50", Description = "50% off all courses", DiscountType = "Percentage", DiscountValue = 50, ValidTo = DateTime.UtcNow.AddMonths(2), CreatedById = userId, MaxUses = 100 },
            new() { Code = "FIRSTBUY", Description = "$15 off your first purchase", DiscountType = "FixedAmount", DiscountValue = 15, ValidTo = DateTime.UtcNow.AddYears(1), CreatedById = userId, MaxUsesPerUser = 1 },
            new() { Code = "WEBDEV25", Description = "25% off Web Development courses", DiscountType = "Percentage", DiscountValue = 25, ValidTo = DateTime.UtcNow.AddMonths(6), CreatedById = userId, CourseId = courses.First(c => c.Title.Contains("ASP.NET")).Id },
            new() { Code = "CLOUD100", Description = "$100 off AWS course", DiscountType = "FixedAmount", DiscountValue = 100, ValidTo = DateTime.UtcNow.AddMonths(1), CreatedById = userId, CourseId = courses.First(c => c.Title.Contains("AWS")).Id, MinPurchaseAmount = 200},
            new() { Code = "EXPIRED", Description = "An expired coupon", DiscountType = "Percentage", DiscountValue = 99, ValidTo = DateTime.UtcNow.AddDays(-1), CreatedById = userId, IsActive = false }
        };
        await _context.Coupons.AddRangeAsync(coupons);
    }

    private async Task SeedCartWithItemsAsync(string userId)
    {
        var existingCart = await _cartRepository.GetCartAsync(userId);
        if (existingCart != null && existingCart.Items.Any())
        {
            return;
        }

        var cart = new Cart(userId);

        var coursesToAdd = await _context.Courses
            .AsNoTracking()
            .Take(2)
            .ToListAsync();

        if (coursesToAdd.Any())
        {
            foreach (var course in coursesToAdd)
            {
                cart.Items.Add(new CartItem
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Price = course.Price,
                    ThumbnailImageUrl = course.ThumbnailImageUrl
                });
            }
        }

        await _cartRepository.UpdateCartAsync(cart);
    }



    private async Task SeedEnrollmentsAsync(string userId)
    {
        if (await _context.Enrollments.AnyAsync()) return;
        var courses = await _context.Courses.Where(c => c.IsPublished).ToListAsync();
        if (!courses.Any()) return;

        // Create enrollments where the userId (student) enrolls in courses created by different instructors
        // This creates the scenario needed for instructor payouts: students paying for instructor courses
        var enrollments = courses.Select(course => new Enrollment { UserId = userId, CourseId = course.Id, PricePaid = course.Price }).ToList();
        await _context.Enrollments.AddRangeAsync(enrollments);
    }

    private async Task SeedCourseRatingsAsync(string userId)
    {
        if (await _context.CourseRatings.AnyAsync()) return;
        var enrollments = await _context.Enrollments.Where(e => e.UserId == userId).ToListAsync();
        if (!enrollments.Any()) return;

        var ratings = new List<CourseRating>
        {
            new() { UserId = userId, CourseId = enrollments[0].CourseId, RatingValue = 5, ReviewText = "Absolutely fantastic course! Highly recommend." },
            new() { UserId = userId, CourseId = enrollments[1].CourseId, RatingValue = 4, ReviewText = "Great content, but could use more practical examples." },
            new() { UserId = userId, CourseId = enrollments[2].CourseId, RatingValue = 5, ReviewText = "The instructor is very clear and knowledgeable." },
            new() { UserId = userId, CourseId = enrollments[3].CourseId, RatingValue = 3, ReviewText = "It was okay, a bit too fast for me." }
        };
        await _context.CourseRatings.AddRangeAsync(ratings);
    }

    private async Task SeedUserBookmarksAsync(string userId)
    {
        if (await _context.UserBookmarks.AnyAsync()) return;
        var courses = await _context.Courses.ToListAsync();
        if (courses.Count < 5) return;

        var bookmarks = new List<UserBookmark>
        {
            new() { UserId = userId, CourseId = courses[0].Id },
            new() { UserId = userId, CourseId = courses[1].Id },
            new() { UserId = userId, CourseId = courses[2].Id },
            new() { UserId = userId, CourseId = courses[3].Id },
            new() { UserId = userId, CourseId = courses[4].Id }
        };
        await _context.UserBookmarks.AddRangeAsync(bookmarks);
    }

    private async Task SeedPaymentsAsync(string userId)
    {
        if (await _context.Payments.AnyAsync()) return;
        var enrollments = await _context.Enrollments.Include(e => e.Course).Where(e => e.UserId == userId).ToListAsync();
        if (!enrollments.Any()) return;

        var payments = enrollments.Select(enrollment => new Payment
        {
            UserId = userId,
            Amount = enrollment.PricePaid,
            PaymentMethod = "Credit Card",
            Status = "Completed",
            TransactionId = Guid.NewGuid().ToString()
        }).ToList();
        await _context.Payments.AddRangeAsync(payments);
    }

    private async Task SeedOrderDetailsAsync()
    {
        if (await _context.OrderDetails.AnyAsync()) return;
        var payments = await _context.Payments.ToListAsync();
        var enrollments = await _context.Enrollments.ToListAsync();
        if (payments.Count != enrollments.Count) return; // Should be the same number

        var orderDetails = new List<OrderDetail>();
        for (int i = 0; i < payments.Count; i++)
        {
            orderDetails.Add(new OrderDetail
            {
                PaymentId = payments[i].Id,
                EnrollmentId = enrollments[i].Id,
                CourseId = enrollments[i].CourseId,
                PriceAtPurchase = enrollments[i].PricePaid
            });
        }
        await _context.OrderDetails.AddRangeAsync(orderDetails);
    }

    private async Task SeedInvoicesAsync(string userId)
    {
        if (await _context.Invoices.AnyAsync()) return;
        var payments = await _context.Payments.Where(p => p.UserId == userId).ToListAsync();
        if (!payments.Any()) return;

        var invoices = payments.Select(p => new Invoice
        {
            UserId = userId,
            PaymentId = p.Id,
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{p.Id}",
            TotalAmount = p.Amount,
            Status = "Paid",
            BillingAddress = "123 Main St, Anytown, USA"
        }).ToList();
        await _context.Invoices.AddRangeAsync(invoices);
    }

    private async Task SeedInvoiceItemsAsync()
    {
        if (await _context.InvoiceItems.AnyAsync()) return;
        var invoices = await _context.Invoices.Include(i => i.Payment).ThenInclude(p => p!.OrderDetails).ThenInclude(od => od.Course).ToListAsync();
        if (!invoices.Any()) return;

        var invoiceItems = new List<InvoiceItem>();
        foreach (var invoice in invoices)
        {
            var orderDetail = invoice.Payment?.OrderDetails.FirstOrDefault();
            if (orderDetail?.Course == null) continue;

            invoiceItems.Add(new InvoiceItem
            {
                InvoiceId = invoice.Id,
                CourseId = orderDetail.Course.Id,
                Description = orderDetail.Course.Title,
                OriginalUnitPrice = orderDetail.Course.Price,
                DiscountAppliedOnItem = orderDetail.Course.Price - orderDetail.PriceAtPurchase,
                UnitPrice = orderDetail.PriceAtPurchase,
            });
        }
        await _context.InvoiceItems.AddRangeAsync(invoiceItems);
    }

    private async Task SeedAppliedCouponsAsync(string userId)
    {
        if (await _context.AppliedCoupons.AnyAsync()) return;
        var payments = await _context.Payments.Where(p => p.UserId == userId).ToListAsync();
        var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == "FIRSTBUY");
        if (payments.Any() && coupon != null)
        {
            await _context.AppliedCoupons.AddAsync(new AppliedCoupon
            {
                CouponId = coupon.Id,
                UserId = userId,
                PaymentId = payments.First().Id,
                DiscountAmountApplied = coupon.DiscountValue
            });
        }
    }
}