using AutoMapper;
using IlmPath.Application.Lectures.Commands.CreateLecture;
using IlmPath.Application.Lectures.Commands.UpdateLecture;
using IlmPath.Application.Lectures.Commands.UpdateLectureOrder;
using IlmPath.Application.Lectures.DTOs;
using IlmPath.Application.Lectures.DTOs.Requests;
using IlmPath.Domain.Entities;

namespace IlmPath.Application.Mappings
{
    public class LectureMappingProfile : Profile
    {
        public LectureMappingProfile()
        {
            CreateMap<Lecture, LectureResponse>();
            CreateMap<CreateLectureCommand, Lecture>();
            CreateMap<CreateLectureRequest, CreateLectureCommand>();

            CreateMap<UpdateLectureRequest, UpdateLectureCommand>();
            CreateMap<UpdateLectureCommand, Lecture>();

            CreateMap<(UpdateLectureOrderRequest, int Id), UpdateLectureOrderCommand>();
        }
    }
} 