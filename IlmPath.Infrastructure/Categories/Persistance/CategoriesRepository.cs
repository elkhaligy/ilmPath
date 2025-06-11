using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IlmPath.Infrastructure.Categories.Persistance;


public class CategoriesRepository : ICategoriesRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }   

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug)
    {
        return await _context.Categories.FirstOrDefaultAsync(cat => cat.Slug == slug);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.Id);
        if (existingCategory == null)
        {
            throw new Exception("Category not found");
        }
        existingCategory.Name = category.Name;
        existingCategory.Slug = category.Slug;
        await _context.SaveChangesAsync();
    }
}