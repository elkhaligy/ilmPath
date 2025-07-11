using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetEnrollmentByIdAsync(int id);
        Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetAllEnrollmentsAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetEnrollmentsByUserIdAsync(string userId, int pageNumber, int pageSize);
        Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetEnrollmentsByInstructorIdAsync(string instructorId, int pageNumber, int pageSize);
        Task<int> GetTotalStudentsCountByInstructorIdAsync(string instructorId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(int id);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);
        Task<Enrollment?> GetEnrollmentByUserAndCourseAsync(string userId, int courseId);
    }
}
