using MediatR;

namespace IlmPath.Application.Lectures.Commands.UpdateLecture
{
    public record UpdateLectureCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int? DurationInMinutes { get; set; }
        public int Order { get; set; }
        public bool IsPreviewAllowed { get; set; }
    }
} 