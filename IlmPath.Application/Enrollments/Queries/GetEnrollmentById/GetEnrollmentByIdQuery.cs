using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Queries.GetEnrollmentById;
public record GetEnrollmentByIdQuery(int Id) : IRequest<Enrollment>;