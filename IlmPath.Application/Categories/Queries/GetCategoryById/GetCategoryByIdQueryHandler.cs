using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.DTOs.Categories.Responses;
using AutoMapper;

namespace IlmPath.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    IMapper _mapper;

    public GetCategoryByIdQueryHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        _mapper=mapper;
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