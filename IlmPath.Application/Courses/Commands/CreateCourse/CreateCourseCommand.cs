using IlmPath.Application.Courses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.CreateCourse
{
    public record CreateCourseCommand(string Title,
    string Description,
    decimal Price,
    string InstructorId,
    int? CategoryId) :IRequest<CourseResponse>;
   
}
