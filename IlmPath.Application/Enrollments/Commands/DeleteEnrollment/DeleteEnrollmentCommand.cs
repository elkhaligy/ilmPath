using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.DeleteEnrollment;

public record DeleteEnrollmentCommand(int id) : IRequest<bool>;
