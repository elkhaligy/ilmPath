using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Categories.Persistance;
using IlmPath.Infrastructure.Data;
using IlmPath.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IlmPath.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<DataSeeder>();
        services.AddScoped<IdentitySeeder>();
        return services;
    }
}
