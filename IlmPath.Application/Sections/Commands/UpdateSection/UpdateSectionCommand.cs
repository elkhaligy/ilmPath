using MediatR;

namespace IlmPath.Application.Sections.Commands.UpdateSection;

public class UpdateSectionCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
} 