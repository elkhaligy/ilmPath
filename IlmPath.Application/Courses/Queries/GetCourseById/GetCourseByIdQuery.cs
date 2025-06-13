using IlmPath.Application.Courses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Queries.GetCourseById
{
    public record GetCourseByIdQuery(int Id):IRequest<CourseResponse>;
   
}
