using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.CreateEnrollment;

public record CreateEnrollmentCommand(string UserId, int CourseId, DateTime EnrollmentDate, decimal PricePaid) : IRequest<Enrollment>;
