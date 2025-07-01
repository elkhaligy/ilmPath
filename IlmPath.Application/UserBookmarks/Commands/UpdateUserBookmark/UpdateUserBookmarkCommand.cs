using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Commands.UpdateUserBookmark;
public record UpdateUserBookmarkCommand(int Id, string UserId, int CourseId, DateTime CreatedAt) : IRequest<UserBookmark>;

