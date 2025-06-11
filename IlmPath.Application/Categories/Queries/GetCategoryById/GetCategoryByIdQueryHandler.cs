using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;

namespace IlmPath.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetCategoryByIdQueryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetCategoryByIdAsync(request.Id);
        
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
        }

        return category;
    }
} 