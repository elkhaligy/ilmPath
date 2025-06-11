using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Interfaces;


public interface ICategoriesRepository
{
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<Category?> GetCategoryBySlugAsync(string slug);
    Task<List<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
}