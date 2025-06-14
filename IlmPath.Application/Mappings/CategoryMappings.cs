using AutoMapper;
using IlmPath.Application.Categories.Commands.UpdateCategory;
using IlmPath.Domain.Entities;
using IlmPath.Application.Categories.Commands.CreateCategory;
using IlmPath.Application.Categories.DTOs.Requests;
using IlmPath.Application.Categories.DTOs.Responses;

namespace IlmPath.Api.Mappings;

public class CategoryMappings : Profile
{
    public CategoryMappings()
    {
        // Domain to Response DTO
        CreateMap<Category, CategoryResponse>();
        CreateMap<CreateCategoryCommand, Category>();


        // Request DTO to Command
        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        
        // For UpdateCourseCommand, we need to handle the Id parameter
        CreateMap<(UpdateCategoryRequest Request, int Id), UpdateCategoryCommand>()
            .ConstructUsing(src => new UpdateCategoryCommand(src.Id, src.Request.Name, src.Request.Slug));
    }
} 