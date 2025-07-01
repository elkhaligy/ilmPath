using IlmPath.Application.Sections.DTOs;
using MediatR;
using System.Collections.Generic;

namespace IlmPath.Application.Sections.Queries.GetSectionsByCourse;

public record GetSectionsByCourseQuery(int CourseId) : IRequest<IEnumerable<SectionResponse>>; 