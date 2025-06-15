using IlmPath.Application.Lectures.DTOs;
using MediatR;

namespace IlmPath.Application.Lectures.Commands.CreateLecture
{
    public record CreateLectureCommand : IRequest<LectureResponse>
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int? DurationInMinutes { get; set; }
        public int Order { get; set; }
        public bool IsPreviewAllowed { get; set; }
    }
} 