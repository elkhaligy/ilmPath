using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(int id);
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize);

        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
    }
}
