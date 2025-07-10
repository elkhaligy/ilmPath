using IlmPath.Application.Common.Pagination;
using IlmPath.Application.CourseRatings.DTOs.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IlmPath.Application.CourseRatings.Queries.GetCourseRatings
{
    public class GetCourseRatingsQuery : IRequest<PagedResult<CourseRatingResponse>>
    {
        [BindNever]
        public int CourseId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [Range(1, 5)]
        public int? RatingFilter { get; set; }
    }
} 