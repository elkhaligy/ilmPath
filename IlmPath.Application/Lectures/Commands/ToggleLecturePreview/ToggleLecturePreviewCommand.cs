using MediatR;

namespace IlmPath.Application.Lectures.Commands.ToggleLecturePreview
{
    public record ToggleLecturePreviewCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public bool IsPreviewAllowed { get; set; }
    }
} 