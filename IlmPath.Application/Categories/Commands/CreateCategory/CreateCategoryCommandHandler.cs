using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Categories.DTOs.Responses;
using AutoMapper;

namespace IlmPath.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        _mapper = mapper;
    }
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Create category
        var category = _mapper.Map<Category>(request);

        // Add it to the db
        await _categoriesRepository.AddCategoryAsync(category);
        // Return category

        return category;
    }
}
