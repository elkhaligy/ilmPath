using AutoMapper;
using IlmPath.Application.CourseRatings.Commands.AddCourseRating;
using IlmPath.Application.CourseRatings.DTOs.Responses;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Mappings
{
    public class CourseRatingMappingProfile : Profile
    {
        public CourseRatingMappingProfile()
        {
            CreateMap<AddCourseRatingCommand, CourseRating>();
            CreateMap<CourseRating, CourseRatingResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl));
        }
    }
} 