using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IlmPath.Infrastructure.Data;
using IlmPath.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Infrastructure.Categories.Persistance;

namespace IlmPath.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        return services;
    }
}
