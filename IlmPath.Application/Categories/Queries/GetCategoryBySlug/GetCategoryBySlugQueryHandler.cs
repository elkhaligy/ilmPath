using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.DTOs.Categories.Responses;
using AutoMapper;

namespace IlmPath.Application.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQueryHandler : IRequestHandler<GetCategoryBySlugQuery, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper _mapper;

    public GetCategoryBySlugQueryHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        _mapper = mapper;
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