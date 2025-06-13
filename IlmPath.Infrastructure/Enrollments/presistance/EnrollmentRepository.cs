using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace IlmPath.Infrastructure.Enrollments.presistance
{
    class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context )
        {
            _context=context;
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
                throw new Exception("Category not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
        {
          return  await _context.Enrollments.FindAsync(id);
        }

        public async Task UpdateEnrollmentAsync(Enrollment enrollment)
        {
            var existingEnrollment = await _context.Enrollments.FindAsync(enrollment.Id);
            if (existingEnrollment == null)
                throw new Exception("Category not found");

            existingEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
            existingEnrollment.PricePaid= enrollment.PricePaid;
            await _context.SaveChangesAsync();
        }
    }
}
