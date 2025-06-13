
using IlmPath.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        if (profileImage == null || profileImage.Length == 0)
        {
            return BadRequest("Image file is required.");
        }

        var command = new UpdateProfileImageCommand
        {
            UserId = userId,
            ProfileImage = profileImage
        };

        var imageUrl = await _mediator.Send(command);

        return Ok(new { ProfileImageUrl = imageUrl });
    }
} 