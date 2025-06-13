
using MediatR;
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
} 