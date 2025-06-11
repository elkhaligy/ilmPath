using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<Category>; 