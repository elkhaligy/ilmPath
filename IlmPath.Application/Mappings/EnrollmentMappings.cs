using AutoMapper;
using IlmPath.Application.Categories.Commands.CreateCategory;
using IlmPath.Application.Categories.Commands.UpdateCategory;
using IlmPath.Application.Categories.DTOs.Requests;
using IlmPath.Application.Categories.DTOs.Responses;
using IlmPath.Application.Enrollments.Commands.CreateEnrollment;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Mappings
{
    class EnrollmentMappings : Profile
    {
        public EnrollmentMappings()
        {
            // Domain to Response DTO
            //CreateMap<Category, CategoryResponse>();
            CreateMap<CreateEnrollmentCommand, Enrollment>();


            // Request DTO to Command
            //CreateMap<CreateCategoryRequest, CreateCategoryCommand>();

            // For UpdateCategoryCommand, we need to handle the Id parameter
            CreateMap<(UpdateCategoryRequest Request, int Id), UpdateCategoryCommand>()
                .ConstructUsing(src => new UpdateCategoryCommand(src.Id, src.Request.Name, src.Request.Slug));
        }
    }
}
