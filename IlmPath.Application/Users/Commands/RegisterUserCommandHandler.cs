using IlmPath.Application.Email;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Users.Commands
{
    // The handler now implements IRequestHandler<RegisterUserCommand, string>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        // The Handle method now returns a Task<string>
        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                // In a real app, you would probably throw a custom validation exception
                throw new Exception("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true // Activate user on registration
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            // Assign a default role, e.g., "User"
            // Ensure the role exists
            if (await _roleManager.RoleExistsAsync("User"))
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            // Send a welcome email
            var emailSubject = "Welcome to IlmPath!";
            var emailBody = $"<p>Dear {user.FirstName},</p>" +
                            "<p>Welcome to IlmPath! We are excited to have you on board.</p>" +
                            "<p>Thank you for registering.</p>" +
                            "<p>Best regards,<br/>The IlmPath Team</p>";

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            // Return the new user's ID instead of a token
            return user.Id;
        }
    }
} 