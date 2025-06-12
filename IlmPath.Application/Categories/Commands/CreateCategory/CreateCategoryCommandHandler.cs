using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.DTOs.Categories.Responses;
using AutoMapper;

namespace IlmPath.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper mapper;

    public CreateCategoryCommandHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        this.mapper = mapper;
    }
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Create lecture
        var category = mapper.Map<Category>(request);

        // Add it to the db
        await _categoriesRepository.AddCategoryAsync(category);
        // Return lecture

        return category;
    }
}
