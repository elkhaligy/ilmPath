
using IlmPath.Application.Users.Commands;
using IlmPath.Application.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using YourApp.Application.Features.Authentication.Commands;

namespace IlmPath.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var query = new GetUserProfileQuery { UserId = userId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { UserId = result });
    }

    [HttpPost("{userId}/profile-image")]
    public async Task<IActionResult> UpdateProfileImage(string userId, IFormFile profileImage)
    {
        
        var command = new UpdateProfileImageCommand
        {
            UserId = userId,
            ProfileImage = profileImage
        };

        var imageUrl = await _mediator.Send(command);

        return Ok(new { ProfileImageUrl = imageUrl });
    }
} 