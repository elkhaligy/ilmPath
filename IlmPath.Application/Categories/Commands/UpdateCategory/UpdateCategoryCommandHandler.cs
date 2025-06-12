using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.DTOs.Categories.Responses;
using AutoMapper;

namespace IlmPath.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper mapper;

    public UpdateCategoryCommandHandler(ICategoriesRepository categoriesRepository, IMapper mapper)
    {
        _categoriesRepository = categoriesRepository;
        this.mapper = mapper;
    }

    public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetCategoryByIdAsync(request.Id);
        
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
        }

        category.Name = request.Name;
        category.Slug = request.Slug;

        await _categoriesRepository.UpdateCategoryAsync(category);

        return category;
    }
} 