using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.UpdateEnrollment;

public record UpdateEnrollmentCommand(int Id, string UserId, int CourseId, DateTime EnrollmentDate, decimal PricePaid) : IRequest<Enrollment>;
