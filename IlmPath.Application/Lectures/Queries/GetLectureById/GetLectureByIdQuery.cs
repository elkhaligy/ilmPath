using IlmPath.Application.Lectures.DTOs;
using MediatR;

namespace IlmPath.Application.Lectures.Queries.GetLectureById
{
    public record GetLectureByIdQuery(int Id) : IRequest<LectureResponse>;
} 