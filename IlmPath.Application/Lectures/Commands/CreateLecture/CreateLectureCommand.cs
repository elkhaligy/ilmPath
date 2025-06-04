using MediatR;

namespace IlmPath.Application.Lectures.Commands.CreateLecture;

public record CreateLectureCommand(string Title, string Description, string VideoUrl, string ImageUrl, string AuthorId) : IRequest<Guid>;
