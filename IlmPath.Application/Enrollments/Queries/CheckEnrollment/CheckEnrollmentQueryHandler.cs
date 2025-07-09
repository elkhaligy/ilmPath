using IlmPath.Application.Common.Interfaces;
using MediatR;

namespace IlmPath.Application.Enrollments.Queries.CheckEnrollment;

public class CheckEnrollmentQueryHandler : IRequestHandler<CheckEnrollmentQuery, CheckEnrollmentResponse>
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public CheckEnrollmentQueryHandler(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<CheckEnrollmentResponse> Handle(CheckEnrollmentQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetEnrollmentByUserAndCourseAsync(request.UserId, request.CourseId);

        return new CheckEnrollmentResponse
        {
            IsEnrolled = enrollment != null,
            EnrollmentDate = enrollment?.EnrollmentDate
        };
    }
} 