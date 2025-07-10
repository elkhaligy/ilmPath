using IlmPath.Application.Users.DTOs.Response;
using MediatR;

namespace IlmPath.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileResponse>
    {
        public string UserId { get; set; }
    }
}