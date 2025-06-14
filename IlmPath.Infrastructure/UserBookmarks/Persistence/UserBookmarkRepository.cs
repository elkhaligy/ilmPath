﻿using AutoMapper;
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

namespace IlmPath.Infrastructure.UserBookmarks.Persistence;

public class UserBookmarkRepository : IUserBookmarkRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserBookmarkRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (userBookmarks, totalCount);
    }

    public async Task<UserBookmark?> GetUserBookmarkByIdAsync(int id)
    {
        return await _context.UserBookmarks.FindAsync(id);
    }

    public async Task UpdateUserBookmarkAsync(UserBookmark userBookmark)
    {
        var existingUserBookmark = await _context.UserBookmarks.FindAsync(userBookmark.Id);
        if (existingUserBookmark == null)
            throw new NotFoundException(nameof(UserBookmark), userBookmark.Id);

        _mapper.Map(userBookmark, existingUserBookmark);

        await _context.SaveChangesAsync();
    }
}
