using IlmPath.Application.Categories.Commands.UpdateCategory;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.UpdateEnrollment;


public class UpdateEnrollmentCommandHandler : IRequestHandler<UpdateEnrollmentCommand, Enrollment>
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public UpdateEnrollmentCommandHandler(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<Enrollment> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(request.Id);

        if (enrollment == null)
        {
            throw new NotFoundException(nameof(Enrollment), request.Id);
        }

        enrollment.EnrollmentDate = request.EnrollmentDate;
        enrollment.PricePaid = request.PricePaid;

        await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);

        return enrollment;
    }
}
