using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Commands.DeleteUserBookmark;
public record DeleteUserBookmarkCommand(int id) : IRequest<bool>;
