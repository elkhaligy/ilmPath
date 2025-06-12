using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using AutoMapper;
using IlmPath.Application.DTOs.Categories.Responses;

namespace IlmPath.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<Category>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    IMapper _mapper;

    public GetAllCategoriesQueryHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        _mapper = mapper;
    }

    public async Task<List<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
      return await _categoriesRepository.GetAllCategoriesAsync();
    }
} 