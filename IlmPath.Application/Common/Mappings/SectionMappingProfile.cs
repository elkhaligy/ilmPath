using AutoMapper;
using IlmPath.Application.Sections.Commands.CreateSection;
using IlmPath.Application.Sections.Commands.UpdateSection;
using IlmPath.Application.Sections.DTOs;
using IlmPath.Application.Sections.DTOs.Requests;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Mappings;

public class SectionMappingProfile : Profile
{
    public SectionMappingProfile()
    {
        CreateMap<Section, SectionResponse>();
        CreateMap<CreateSectionRequest, CreateSectionCommand>();
        CreateMap<UpdateSectionRequest, UpdateSectionCommand>();
    }
} 