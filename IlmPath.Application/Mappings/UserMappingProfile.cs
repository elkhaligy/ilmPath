using AutoMapper;
using IlmPath.Application.Users.DTOs.Response;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserProfileResponse>()
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfileImageUrl));
        }
    }
} 