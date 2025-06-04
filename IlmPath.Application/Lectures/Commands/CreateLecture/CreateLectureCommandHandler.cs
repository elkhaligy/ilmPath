using MediatR;

namespace IlmPath.Application.Lectures.Commands.CreateLecture;

public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, Guid>
{
    public Task<Guid> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
