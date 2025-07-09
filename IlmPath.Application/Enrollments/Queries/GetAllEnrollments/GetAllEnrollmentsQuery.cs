using IlmPath.Application.Common.Pagination;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Queries.GetAllEnrollments;
public record GetAllEnrollmentsQuery(int PageNumber = 1, int PageSize = 10, string? UserId = null) : IRequest<(IEnumerable<Enrollment>, int count)>;
