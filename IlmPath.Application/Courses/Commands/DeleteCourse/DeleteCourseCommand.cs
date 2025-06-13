using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.DeleteCourse
{
    public record DeleteCourseCommand(int Id) : IRequest<Unit>;

}
