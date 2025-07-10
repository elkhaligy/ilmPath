using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Users.DTOs.Response;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            return _mapper.Map<UserProfileResponse>(user);
        }
    }
} 