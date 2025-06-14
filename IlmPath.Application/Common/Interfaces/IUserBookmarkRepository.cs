using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces;

public interface IUserBookmarkRepository
{
    Task<UserBookmark?> GetUserBookmarkByIdAsync(int id);
    Task<(IEnumerable<UserBookmark> userBookmarks, int TotalCount)> GetAllUserBookmarksAsync(int pageNumber, int pageSize);
    Task AddUserBookmarkAsync(UserBookmark userBookmark);
    Task UpdateUserBookmarkAsync(UserBookmark userBookmark);
    Task DeleteUserBookmarkAsync(int id);
}
