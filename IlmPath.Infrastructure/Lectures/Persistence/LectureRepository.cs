using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Lectures.Persistence
{
    public class LectureRepository : ILectureRepository
    {
        private readonly ApplicationDbContext _context;

        public LectureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Lecture> AddAsync(Lecture lecture)
        {
            await _context.Lectures.AddAsync(lecture);
            await _context.SaveChangesAsync();
            return lecture;
        }

        public async Task DeleteAsync(int id)
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Lecture> GetByIdAsync(int id)
        {
            return await _context.Lectures.FindAsync(id);
        }

        public async Task<IEnumerable<Lecture>> GetBySectionIdAsync(int sectionId)
        {
            return await _context.Lectures
                                 .Where(l => l.SectionId == sectionId)
                                 .OrderBy(l => l.Order)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(Lecture lecture)
        {
            _context.Entry(lecture).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
} 