using IlmPath.Application.Sections.DTOs;
using MediatR;

namespace IlmPath.Application.Sections.Queries.GetSectionById;

public record GetSectionByIdQuery(int Id) : IRequest<SectionResponse>; 