using MediatR;
using IlmPath.Domain.Entities;
using IlmPath.Application.Common.Interfaces;

namespace IlmPath.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoriesRepository _categoriesRepository;
    public CreateCategoryCommandHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Create lecture
        var category = new Category
        {
            Name = request.Name,
            Slug = request.Slug
        };

        // Add it to the db
        await _categoriesRepository.AddCategoryAsync(category);
        // Return lecture

        return await Task.FromResult(category);
    }
}
