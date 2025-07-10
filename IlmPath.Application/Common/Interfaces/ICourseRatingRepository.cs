using IlmPath.Application.Common.Pagination;
using IlmPath.Domain.Entities;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface ICourseRatingRepository
    {
        Task<CourseRating> AddRatingAsync(CourseRating rating);
        Task<bool> HasUserRatedCourseAsync(string userId, int courseId);
        Task<PagedResult<CourseRating>> GetRatingsForCourseAsync(int courseId, int pageNumber, int pageSize, int? ratingFilter);
        Task<CourseRating?> GetRatingByIdAsync(int ratingId);
        Task DeleteRatingAsync(int ratingId);
    }
} 