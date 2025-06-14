using AutoMapper;
using IlmPath.Application.Enrollments.Commands.CreateEnrollment;
using IlmPath.Application.Enrollments.Commands.UpdateEnrollment;
using IlmPath.Application.Enrollments.DTOs.Requests;
using IlmPath.Application.Enrollments.DTOs.Responses;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Mappings;

class EnrollmentMappings : Profile
{
    public EnrollmentMappings()
    {
        // Domain to Response DTO
        CreateMap<Enrollment, EnrollmentResponse>();
        CreateMap<CreateEnrollmentCommand, Enrollment>();


        // Request DTO to Command
        CreateMap<CreateEnrollmentRequest, CreateEnrollmentCommand>();

        // For UpdateCategoryCommand, we need to handle the Id parameter
        CreateMap<(UpdateEnrollmentRequest Request, int Id), UpdateEnrollmentCommand>()
            .ConstructUsing(src => new UpdateEnrollmentCommand(src.Id, src.Request.EnrollmentDate, src.Request.PricePaid));
    }
}
