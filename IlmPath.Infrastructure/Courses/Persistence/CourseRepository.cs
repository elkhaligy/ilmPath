﻿using Azure.Core;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Courses.Persistence
{
    public class CourseRepository(ApplicationDbContext _context) : ICourseRepository
    {
        public async Task<Course> AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;

        }

        public async Task DeleteAsync(int id)
        {
            var course= await _context.Courses.FindAsync(id);
            if(course != null) 
            {   _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException(nameof(Course), id);
            }

        }

        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string? searchQuery = null)
        {
            var query = _context.Courses.AsQueryable();

            // Apply search filter if search query is provided
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(c => c.Title.Contains(searchQuery) || c.Description.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();

            var courses = await query
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.CreatedAt) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
           
            return (courses, totalCount);
        }

        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize)
        {
            var query = _context.Courses
                .Where(c => c.CategoryId == categoryId);

            var totalCount = await query.CountAsync();

            var courses = await query
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.CreatedAt) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalCount);
        }

        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetByInstructorIdAsync(string instructorId, int pageNumber, int pageSize)
        {
            var query = _context.Courses
                .Where(c => c.InstructorId == instructorId);

            var totalCount = await query.CountAsync();

            var courses = await query
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.CreatedAt) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalCount);
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
            .Include(c => c.Category) 
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Course course)
        {
 
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
