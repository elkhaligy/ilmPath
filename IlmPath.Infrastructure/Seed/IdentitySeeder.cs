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

        if (await _userManager.Users.AnyAsync()) return;
        
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roleNames = { "Admin", "User" };

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

        // Seed Instructor 1
        if (await _userManager.FindByNameAsync("instructor1") == null)
        {
            var instructor1 = new ApplicationUser
            {
                UserName = "instructor1",
                Email = "instructor1@ilmpath.com",
                FirstName = "Instructor",
                LastName = "One",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(instructor1, "Instructor1@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(instructor1, "User");
            }
        }

        // Seed Instructor 2
        if (await _userManager.FindByNameAsync("instructor2") == null)
        {
            var instructor2 = new ApplicationUser
            {
                UserName = "instructor2",
                Email = "instructor2@ilmpath.com",
                FirstName = "Instructor",
                LastName = "Two",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(instructor2, "Instructor2@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(instructor2, "User");
            }
        }

        // Seed Student 1
        if (await _userManager.FindByNameAsync("student1") == null)
        {
            var student1 = new ApplicationUser
            {
                UserName = "student1",
                Email = "student1@ilmpath.com",
                FirstName = "Student",
                LastName = "One",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(student1, "Student1@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student1, "User");
            }
        }

        // Seed Student 2
        if (await _userManager.FindByNameAsync("student2") == null)
        {
            var student2 = new ApplicationUser
            {
                UserName = "student2",
                Email = "student2@ilmpath.com",
                FirstName = "Student",
                LastName = "Two",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(student2, "Student2@123");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student2, "User");
            }
        }
    }
}
