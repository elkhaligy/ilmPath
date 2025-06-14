
using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Invoices.Commands.Delete_Invoice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Commands.DeleteUserBookmark;

public class DeleteUserBookmarkCommandHandler : IRequestHandler<DeleteUserBookmarkCommand, bool>
{
    private readonly IUserBookmarkRepository _userBookmarkRepository;
    private readonly IMapper _mapper;
    public DeleteUserBookmarkCommandHandler(IUserBookmarkRepository userBookmarkRepository, IMapper mapper)
    {
        _userBookmarkRepository = userBookmarkRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteUserBookmarkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _userBookmarkRepository.DeleteUserBookmarkAsync(request.id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}