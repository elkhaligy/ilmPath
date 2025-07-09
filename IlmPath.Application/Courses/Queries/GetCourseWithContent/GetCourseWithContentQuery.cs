using IlmPath.Application.Courses.DTOs;
using MediatR;

namespace IlmPath.Application.Courses.Queries.GetCourseWithContent
{
    public record GetCourseWithContentQuery(int CourseId) : IRequest<CourseWithContentResponse>;
} 