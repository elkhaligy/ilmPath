using AutoMapper;
using IlmPath.Application.Courses.Commands.CreateCourse;
using IlmPath.Application.Courses.Commands.UpdateCourse;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : null));

            CreateMap<CreateCourseCommand, Course>();
            CreateMap<UpdateCourseCommand, Course>();
            
            // New mapping for course with content
            CreateMap<Course, CourseWithContentResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : null))
                .ForMember(dest => dest.Sections, opt => opt.Ignore()) // Will be mapped manually in handler
                .ForMember(dest => dest.TotalDurationMinutes, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLecturesCount, opt => opt.Ignore())
                .ForMember(dest => dest.SectionsCount, opt => opt.Ignore());
        }
    }
}
