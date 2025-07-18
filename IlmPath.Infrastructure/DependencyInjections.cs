using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.AppliedCoupons.Persistence;
using IlmPath.Infrastructure.Carts;
using IlmPath.Infrastructure.Categories.Persistence;
using IlmPath.Infrastructure.CourseRatings.Persistence;
using IlmPath.Infrastructure.Courses.Persistence;
using IlmPath.Infrastructure.Data;
using IlmPath.Infrastructure.Enrollments.Persistence;
using IlmPath.Infrastructure.InvoiceItems.Persistence;
using IlmPath.Infrastructure.Invoices.Persistence;
using IlmPath.Infrastructure.Lectures.Persistence;
using IlmPath.Infrastructure.OrderDetails.Persistence;
using IlmPath.Infrastructure.Payouts.Persistence;
using IlmPath.Infrastructure.Payments.Persistence;
using IlmPath.Infrastructure.Sections.Persistence;
using IlmPath.Infrastructure.Seed;
using IlmPath.Infrastructure.Services;
using IlmPath.Infrastructure.UserBookmarks.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
namespace IlmPath.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, IdentityRole>(options => 
        {
            // Disable ALL password requirements
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;                 // Minimum possible
            options.Password.RequiredUniqueChars = 1;            // Minimum possible
            
            // Optional: Keep email unique
            options.User.RequireUniqueEmail = true;
        })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            //options.SaveToken = true;
            //options.RequireHttpsMetadata = false; // Set to true in production
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });


        // For Redis
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConfiguration = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(redisConfiguration);
        });


        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<DataSeeder>();
        services.AddScoped<IdentitySeeder>();
        services.AddScoped<ICourseRepository,CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
        services.AddScoped<IUserBookmarkRepository, UserBookmarkRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IAppliedCouponRepository, AppliedCouponRepository>();
        services.AddScoped<ICourseRatingRepository, CourseRatingRepository>();
        services.AddScoped<IInstructorPayoutRepository, InstructorPayoutRepository>();


        services.AddScoped<ICartRepository, RedisCartRepository>();
        
        // Services
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<IStripeConnectService, StripeConnectService>();
        services.AddScoped<IVideoUploadService, GoogleCloudStorageService>();
        
        return services;
    }
}
