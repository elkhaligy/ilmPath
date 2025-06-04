using MediatR;
using FluentResults;
namespace IlmPath.Application.Lectures.Commands.CreateLecture;

public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok(Guid.NewGuid()));
    }
}
