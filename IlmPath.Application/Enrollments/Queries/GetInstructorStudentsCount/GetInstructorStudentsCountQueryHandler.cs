using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Queries.GetInstructorStudentsCount
{
    public class GetInstructorStudentsCountQueryHandler : IRequestHandler<GetInstructorStudentsCountQuery, int>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public GetInstructorStudentsCountQueryHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<int> Handle(GetInstructorStudentsCountQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository.GetTotalStudentsCountByInstructorIdAsync(request.InstructorId);
        }
    }
} 