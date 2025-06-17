using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Sections.Persistence;

public class SectionRepository : ISectionRepository
{
    private readonly ApplicationDbContext _context;

    public SectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Section> AddAsync(Section section)
    {
        await _context.Sections.AddAsync(section);
        await _context.SaveChangesAsync();
        return section;
    }

    public async Task<Section> GetByIdAsync(int id)
    {
        return await _context.Sections.FindAsync(id);
    }

    public async Task<IEnumerable<Section>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Sections
            .Where(s => s.CourseId == courseId)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task UpdateAsync(Section section)
    {
        _context.Entry(section).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var section = await _context.Sections.FindAsync(id);
        if (section != null)
        {
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }
    }
} 