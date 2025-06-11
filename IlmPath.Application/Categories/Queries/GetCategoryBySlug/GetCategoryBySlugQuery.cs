using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Queries.GetCategoryBySlug;

public record GetCategoryBySlugQuery(string Slug) : IRequest<Category>; 