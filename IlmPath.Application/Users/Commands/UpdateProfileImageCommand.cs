using MediatR;
using Microsoft.AspNetCore.Http;

namespace IlmPath.Application.Users.Commands;

public class UpdateProfileImageCommand : IRequest<string>
{
    public required string UserId { get; set; }
    public required IFormFile ProfileImage { get; set; }
}
