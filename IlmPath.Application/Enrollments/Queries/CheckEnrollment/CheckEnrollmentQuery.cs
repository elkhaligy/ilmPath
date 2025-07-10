using MediatR;

namespace IlmPath.Application.Enrollments.Queries.CheckEnrollment;

public record CheckEnrollmentQuery(string UserId, int CourseId) : IRequest<CheckEnrollmentResponse>; 