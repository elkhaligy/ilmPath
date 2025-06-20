using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;

namespace IlmPath.Application.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQueryHandler : IRequestHandler<GetCategoryBySlugQuery, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetCategoryBySlugQueryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Category> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetCategoryBySlugAsync(request.Slug);
        
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with slug '{request.Slug}' not found.");
        }

        return category;
    }
} 