using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Queries.GetAllUserBookmarks;

public class GetAllUserBookmarksQueryHandler : IRequestHandler<GetAllUserBookmarksQuery, (IEnumerable<UserBookmark>, int count)>
{
    private readonly IUserBookmarkRepository _userBookmarkRepository;
    public GetAllUserBookmarksQueryHandler(IUserBookmarkRepository userBookmarkRepository)
    {
        _userBookmarkRepository = userBookmarkRepository;
    }

    public async Task<(IEnumerable<UserBookmark>, int count)> Handle(GetAllUserBookmarksQuery request, CancellationToken cancellationToken)
    {
        var (userBookmarks, totalCount) = await _userBookmarkRepository.GetAllUserBookmarksAsync(request.PageNumber, request.PageSize);
        return (userBookmarks, totalCount);
    }
}