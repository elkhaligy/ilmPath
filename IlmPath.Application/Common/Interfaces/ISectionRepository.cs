using IlmPath.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces;

public interface ISectionRepository
{
    Task<Section> AddAsync(Section section);
    Task<Section> GetByIdAsync(int id);
    Task<IEnumerable<Section>> GetByCourseIdAsync(int courseId);
    Task UpdateAsync(Section section);
    Task DeleteAsync(int id);
} 