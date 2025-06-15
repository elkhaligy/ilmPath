using MediatR;

namespace YourApp.Application.Features.Authentication.Commands;


public class LoginCommand : IRequest<TokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}