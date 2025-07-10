using AutoMapper;
using IlmPath.Application.Sections.Commands.CreateSection;
using IlmPath.Application.Sections.Commands.UpdateSection;
using IlmPath.Application.Sections.DTOs;
using IlmPath.Application.Sections.DTOs.Requests;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Mappings
{
    public class SectionMappingProfile : Profile
    {
        public SectionMappingProfile()
        {
            CreateMap<Section, SectionResponse>();
            CreateMap<CreateSectionCommand, Section>();
            CreateMap<CreateSectionRequest, CreateSectionCommand>();

            CreateMap<UpdateSectionRequest, UpdateSectionCommand>();
            CreateMap<UpdateSectionCommand, Section>();
            
            // New mapping for section with lectures
            CreateMap<Section, SectionWithLecturesResponse>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => "")) // Default empty description
                .ForMember(dest => dest.Lectures, opt => opt.Ignore()) // Will be mapped manually in handler
                .ForMember(dest => dest.DurationMinutes, opt => opt.Ignore())
                .ForMember(dest => dest.LecturesCount, opt => opt.Ignore());
        }
    }
} 