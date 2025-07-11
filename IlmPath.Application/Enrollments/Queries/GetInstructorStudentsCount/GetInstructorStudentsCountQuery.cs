using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Queries.GetInstructorStudentsCount
{
    public record GetInstructorStudentsCountQuery(string InstructorId) : IRequest<int>;
} 