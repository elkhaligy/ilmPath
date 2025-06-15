using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Queries.GetEnrollmentById;

public class GetEnrollmentByIdQueyHandler : IRequestHandler<GetEnrollmentByIdQuery, Enrollment>
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public GetEnrollmentByIdQueyHandler(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<Enrollment> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(request.Id);

        if (enrollment == null)
        {
            throw new NotFoundException(nameof(Enrollment), request.Id);
        }

        return enrollment;
    }
}
