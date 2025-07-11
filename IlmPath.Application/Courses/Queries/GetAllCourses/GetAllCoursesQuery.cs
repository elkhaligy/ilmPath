using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Courses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Queries.GetAllCourses
{
    // momkn ba3d kda n7ot pagination 3lshan kda 3mlt record leha 
    public record GetAllCoursesQuery(int PageNumber = 1, int PageSize = 10, string? SearchQuery = null) : IRequest<PagedResult<CourseResponse>>;
  
}
