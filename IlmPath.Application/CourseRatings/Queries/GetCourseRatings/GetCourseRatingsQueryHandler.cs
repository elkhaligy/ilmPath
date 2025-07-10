using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.CourseRatings.DTOs.Responses;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.CourseRatings.Queries.GetCourseRatings
{
    public class GetCourseRatingsQueryHandler : IRequestHandler<GetCourseRatingsQuery, PagedResult<CourseRatingResponse>>
    {
        private readonly ICourseRatingRepository _courseRatingRepository;
        private readonly IMapper _mapper;

        public GetCourseRatingsQueryHandler(ICourseRatingRepository courseRatingRepository, IMapper mapper)
        {
            _courseRatingRepository = courseRatingRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CourseRatingResponse>> Handle(GetCourseRatingsQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _courseRatingRepository.GetRatingsForCourseAsync(
                request.CourseId,
                request.PageNumber,
                request.PageSize,
                request.RatingFilter);

            var response = _mapper.Map<List<CourseRatingResponse>>(pagedResult.Items);

            return new PagedResult<CourseRatingResponse>(response, pagedResult.TotalCount, request.PageNumber, request.PageSize);
        }
    }
} 