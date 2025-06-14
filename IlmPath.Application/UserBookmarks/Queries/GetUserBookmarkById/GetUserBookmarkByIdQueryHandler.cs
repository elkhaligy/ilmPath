using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Queries.GetUserBookmarkById;

public class GetUserBookmarkByIdQueryHandler : IRequestHandler<GetUserBookmarkByIdQuery, UserBookmark>
{
    private readonly IUserBookmarkRepository _userBookmarkRepository;
    public GetUserBookmarkByIdQueryHandler(IUserBookmarkRepository userBookmarkRepository)
    {
        _userBookmarkRepository = userBookmarkRepository;
    }


    public async Task<UserBookmark> Handle(GetUserBookmarkByIdQuery request, CancellationToken cancellationToken)
    {
        var userBookmark = await _userBookmarkRepository.GetUserBookmarkByIdAsync(request.Id);

        if (userBookmark == null)
        {
            throw new NotFoundException(nameof(UserBookmark), request.Id);
        }

        return userBookmark;
    }
}


