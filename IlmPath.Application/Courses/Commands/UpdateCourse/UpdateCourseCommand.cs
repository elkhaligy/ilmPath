using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.UpdateCourse
{
    public record UpdateCourseCommand(int Id,
    string Title,
    string Description,
    decimal Price,
    bool IsPublished,
    string? ThumbnailImageUrl,
    int? CategoryId) : IRequest<Unit>;
   
}
