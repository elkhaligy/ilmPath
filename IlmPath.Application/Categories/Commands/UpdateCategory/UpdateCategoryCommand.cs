using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(int Id, string Name, string Slug) : IRequest<Category>; 