using MediatR;

namespace IlmPath.Application.Users.Commands
{
    // The command now specifies a 'string' return type, which will be the user's ID.
    public class RegisterUserCommand : IRequest<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}