﻿using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.UserBookmarks.Queries.GetAllUserBookmarks;

public record GetAllUserBookmarksQuery(int PageNumber = 1, int PageSize = 10) : IRequest<(IEnumerable<UserBookmark>, int count)>;

