using AutoMapper;
using IlmPath.Application.UserBookmarks.Commands.CreateUserBookmark;
using IlmPath.Application.UserBookmarks.Commands.UpdateUserBookmark;
using IlmPath.Application.UserBookmarks.DTOs.Requests;
using IlmPath.Application.UserBookmarks.DTOs.Responses;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Mappings;
 
class UserBookmarkMappings : Profile
{
    public UserBookmarkMappings()
    {
        // Domain to Response DTO
        CreateMap<UserBookmark, UserBookmarkResponse>();

        //command to Domain
        CreateMap<CreateUserBookmarkCommand, UserBookmark>();
        CreateMap<UpdateUserBookmarkCommand, UserBookmark>();


        // Domain to Domain for Update
        CreateMap<UserBookmark, UserBookmark>();


        // Request DTO to Command
        CreateMap<CreateUserBookmarkRequest, CreateUserBookmarkCommand>();

        //For UpdateUserBookmarkCommand, we need to handle the Id parameter
        CreateMap<(UpdateUserBookmarkRequest Request, int Id), UpdateUserBookmarkCommand>()
            .ConstructUsing(src => new UpdateUserBookmarkCommand(src.Id, src.Request.UserId, src.Request.CourseId, src.Request.CreatedAt));
    }
}
