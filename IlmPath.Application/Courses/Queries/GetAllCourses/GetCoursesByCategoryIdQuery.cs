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
    public record GetCoursesByCategoryIdQuery(int CategoryId, int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<CourseResponse>>;

}
