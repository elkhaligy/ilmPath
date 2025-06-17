using MediatR;

namespace IlmPath.Application.Sections.Commands.DeleteSection;

public record DeleteSectionCommand(int Id) : IRequest; 