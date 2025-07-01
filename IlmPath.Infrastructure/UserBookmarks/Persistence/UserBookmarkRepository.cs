using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.UserBookmarks.Persistence;

public class UserBookmarkRepository : IUserBookmarkRepository
{
    private readonly ApplicationDbContext _context;

    public UserBookmarkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddUserBookmarkAsync(UserBookmark userBookmark)
    {
        await _context.UserBookmarks.AddAsync(userBookmark);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserBookmarkAsync(int id)
    {
        var userBookmark = await _context.UserBookmarks.FindAsync(id);
        if (userBookmark == null)
            throw new NotFoundException(nameof(UserBookmark), id);

        _context.UserBookmarks.Remove(userBookmark);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<UserBookmark> userBookmarks, int TotalCount)> GetAllUserBookmarksAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.UserBookmarks.CountAsync();

        var userBookmarks = await _context.UserBookmarks
        .Include(u=> u.User)
        .Include(u => u.Course)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (userBookmarks, totalCount);
    }

    public async Task<UserBookmark?> GetUserBookmarkByIdAsync(int id)
    {
        return await _context.UserBookmarks
        .Include(u=> u.Course)
        .Include(u=> u.User)
        .FirstOrDefaultAsync(u=> u.Id == id);
    }

    public async Task UpdateUserBookmarkAsync(UserBookmark userBookmark)
    {
        var existingUserBookmark = await _context.UserBookmarks.FindAsync(userBookmark.Id);
        if (existingUserBookmark == null)
            throw new NotFoundException(nameof(UserBookmark), userBookmark.Id);

        existingUserBookmark.UserId = userBookmark.UserId;
        existingUserBookmark.CourseId = userBookmark.CourseId;
        existingUserBookmark.CreatedAt = userBookmark.CreatedAt;

                await _context.SaveChangesAsync();
    }
}
