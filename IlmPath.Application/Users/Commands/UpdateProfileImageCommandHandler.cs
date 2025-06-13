using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Users.Commands;

public class UpdateProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UpdateProfileImageCommandHandler(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> Handle(UpdateProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (request.ProfileImage == null || request.ProfileImage.Length == 0)
        {
            throw new Exception("Image file is missing or empty.");
        }

        // Define a path to save the image. e.g., wwwroot/images/profiles
        var uploadsFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
        if (!Directory.Exists(uploadsFolderPath))
        {
            Directory.CreateDirectory(uploadsFolderPath);
        }

        // Create a unique filename
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfileImage.FileName)}";
        var filePath = Path.Combine(uploadsFolderPath, fileName);

        // Save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.ProfileImage.CopyToAsync(stream, cancellationToken);
        }

        // Generate the URL to be stored in the database
        // This URL should be accessible from the client.
        // The base path should be configured, but for simplicity, we'll construct it here.
        var imageUrl = $"/images/profiles/{fileName}";

        // Update user's profile image URL
        user.ProfileImageUrl = imageUrl;
        user.LastModifiedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            // Clean up the uploaded file if the user update fails
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            throw new Exception("Failed to update user profile.");
        }

        return imageUrl;
    }
}