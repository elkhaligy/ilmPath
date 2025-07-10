using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace IlmPath.Infrastructure.Enrollments.Persistence
{
    class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context )
        {
            _context = context;
        }
        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            var enrollment=  await _context.Enrollments.FindAsync(id);
            if(enrollment == null)
                throw new NotFoundException(nameof(Enrollment),id);
            
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

        }

        public async Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetAllEnrollmentsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Enrollments.CountAsync();

            var enrollments = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (enrollments, totalCount);
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            var existingEnrollment = await _context.Enrollments.FindAsync(enrollment.Id);
            if (existingEnrollment == null)
                throw new NotFoundException(nameof(Enrollment), enrollment.Id);

            existingEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
            existingEnrollment.PricePaid= enrollment.PricePaid;
            existingEnrollment.UserId = enrollment.UserId;
            existingEnrollment.CourseId = enrollment.CourseId;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<Enrollment?> GetEnrollmentByUserAndCourseAsync(string userId, int courseId)
        {
            return await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetEnrollmentsByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
            var query = _context.Enrollments
                .Where(e => e.UserId == userId);

            var totalCount = await query.CountAsync();

            var enrollments = await query
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (enrollments, totalCount);
        }

        public async Task<(IEnumerable<Enrollment> enrollments, int TotalCount)> GetEnrollmentsByInstructorIdAsync(string instructorId, int pageNumber, int pageSize)
        {
            var query = _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Where(e => e.Course!.InstructorId == instructorId);

            var totalCount = await query.CountAsync();

            var enrollments = await query
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (enrollments, totalCount);
        }

        public async Task<int> GetTotalStudentsCountByInstructorIdAsync(string instructorId)
        {
            // Get count of unique students enrolled in this instructor's courses
            var uniqueStudentCount = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.Course!.InstructorId == instructorId)
                .Select(e => e.UserId)
                .Distinct()
                .CountAsync();

            return uniqueStudentCount;
        }
    }
}
