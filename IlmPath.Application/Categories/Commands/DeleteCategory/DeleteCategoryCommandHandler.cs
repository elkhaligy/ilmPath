using MediatR;
using IlmPath.Application.Common.Interfaces;

namespace IlmPath.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public DeleteCategoryCommandHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _categoriesRepository.DeleteCategoryAsync(request.Id);
            return true;
        }
        catch
        {
            return false;
        }
    }
} 