using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery() : IRequest<List<Category>>; 