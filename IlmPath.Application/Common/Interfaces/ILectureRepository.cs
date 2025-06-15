using IlmPath.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface ILectureRepository
    {
        Task<Lecture> GetByIdAsync(int id);
        Task<IEnumerable<Lecture>> GetBySectionIdAsync(int sectionId);
        Task<Lecture> AddAsync(Lecture lecture);
        Task UpdateAsync(Lecture lecture);
        Task DeleteAsync(int id);
    }
} 