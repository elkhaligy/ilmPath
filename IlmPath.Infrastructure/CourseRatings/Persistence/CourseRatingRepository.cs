using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Pagination;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.CourseRatings.Persistence
{
    public class CourseRatingRepository : ICourseRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CourseRating> AddRatingAsync(CourseRating rating)
        {
            await _context.CourseRatings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> HasUserRatedCourseAsync(string userId, int courseId)
        {
            return await _context.CourseRatings
                                 .AnyAsync(r => r.UserId == userId && r.CourseId == courseId);
        }

        public async Task<PagedResult<CourseRating>> GetRatingsForCourseAsync(int courseId, int pageNumber, int pageSize, int? ratingFilter)
        {
            var query = _context.CourseRatings
                                .Where(r => r.CourseId == courseId)
                                .Include(r => r.User) // Include user details
                                .AsQueryable();

            if (ratingFilter.HasValue)
            {
                query = query.Where(r => r.RatingValue == ratingFilter.Value);
            }

            var totalCount = await query.CountAsync();

            var ratings = await query.OrderByDescending(r => r.CreatedAt)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            return new PagedResult<CourseRating>(ratings, totalCount, pageNumber, pageSize);
        }

        public async Task<CourseRating?> GetRatingByIdAsync(int ratingId)
        {
            return await _context.CourseRatings.FindAsync(ratingId);
        }

        public async Task DeleteRatingAsync(int ratingId)
        {
            var rating = await _context.CourseRatings.FindAsync(ratingId);
            if (rating == null)
            {
                throw new NotFoundException(nameof(CourseRating), ratingId);
            }

            _context.CourseRatings.Remove(rating);
            await _context.SaveChangesAsync();
        }
    }
} 