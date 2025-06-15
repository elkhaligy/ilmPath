using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Commands.CreateUserBookmark;
public class CreateUserBookmarkCommandHandler : IRequestHandler<CreateUserBookmarkCommand, UserBookmark>
{
    private readonly IUserBookmarkRepository _userBookmarkRepository;
    private readonly IMapper _mapper;


    public CreateUserBookmarkCommandHandler(IUserBookmarkRepository userBookmarkRepository, IMapper mapper)
    {
        _userBookmarkRepository = userBookmarkRepository;
        _mapper = mapper;
    }

    public async Task<UserBookmark> Handle(CreateUserBookmarkCommand request, CancellationToken cancellationToken)
    {
        // Create userBookmark
        var userBookmark = _mapper.Map<UserBookmark>(request);

        // Add it to the db
        await _userBookmarkRepository.AddUserBookmarkAsync(userBookmark);

        // Return userBookmark
        return userBookmark;
    }
}
