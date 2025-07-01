using MediatR;

namespace IlmPath.Application.Lectures.Commands.DeleteLecture
{
    public record DeleteLectureCommand(int Id) : IRequest<Unit>;
} 