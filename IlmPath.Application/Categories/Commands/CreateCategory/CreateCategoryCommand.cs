using FluentResults;
using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string Slug) : IRequest<Category>;
