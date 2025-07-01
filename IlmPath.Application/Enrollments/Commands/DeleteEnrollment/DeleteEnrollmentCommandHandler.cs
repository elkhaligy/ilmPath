using IlmPath.Application.Categories.Commands.DeleteCategory;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.DeleteEnrollment;

public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand, bool>
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public DeleteEnrollmentCommandHandler(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<bool> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _enrollmentRepository.DeleteEnrollmentAsync(request.id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
