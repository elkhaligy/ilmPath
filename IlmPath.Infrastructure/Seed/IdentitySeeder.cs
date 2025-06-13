using IlmPath.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Seed;

public class IdentitySeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentitySeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        

        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roleNames = { "Admin", "Teacher", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        // Seed Admin User
        if (await _userManager.FindByNameAsync("admin") == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@ilmpath.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed Teacher User
        if (await _userManager.FindByNameAsync("teacher") == null)
        {
            var teacherUser = new ApplicationUser
            {
                UserName = "teacher",
                Email = "teacher@ilmpath.com",
                FirstName = "Teacher",
                LastName = "User",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(teacherUser, "Teacher@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacherUser, "Teacher");
            }
        }

        // Seed Basic User
        if (await _userManager.FindByNameAsync("user") == null)
        {
            var basicUser = new ApplicationUser
            {
                UserName = "user",
                Email = "user@ilmpath.com",
                FirstName = "Basic",
                LastName = "User",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(basicUser, "User@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(basicUser, "User");
            }
        }
    }
}
