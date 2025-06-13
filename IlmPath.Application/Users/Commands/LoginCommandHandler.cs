using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YourApp.Application.Features.Authentication.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
        
    }

    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the user
        var user = await _userManager.FindByEmailAsync(request.Email);
        


        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            // In a real app, you might use a more specific custom exception
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // 2. Build the claims
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.NameIdentifier,user.Id),
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        // 3. Generate the token
        var token = CreateToken(authClaims);
        var tokenHandler = new JwtSecurityTokenHandler();

        // 4. Return the response DTO
        return new TokenResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}