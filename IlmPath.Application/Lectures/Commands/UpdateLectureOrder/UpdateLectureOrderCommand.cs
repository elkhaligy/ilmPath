using MediatR;

namespace IlmPath.Application.Lectures.Commands.UpdateLectureOrder
{
    public record UpdateLectureOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
} 