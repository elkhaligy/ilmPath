using IlmPath.Application.Lectures.DTOs;
using MediatR;
using System.Collections.Generic;

namespace IlmPath.Application.Lectures.Queries.GetLecturesBySection
{
    public record GetLecturesBySectionQuery(int SectionId) : IRequest<IEnumerable<LectureResponse>>;
} 