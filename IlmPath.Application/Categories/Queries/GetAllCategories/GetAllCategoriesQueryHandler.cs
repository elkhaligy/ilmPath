using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;


namespace IlmPath.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<Category>>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetAllCategoriesQueryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<List<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
      return await _categoriesRepository.GetAllCategoriesAsync();
    }
} 