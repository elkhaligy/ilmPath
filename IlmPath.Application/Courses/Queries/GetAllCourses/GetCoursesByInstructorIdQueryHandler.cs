using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Courses.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Queries.GetAllCourses
{
    public class GetCoursesByInstructorIdQueryHandler : IRequestHandler<GetCoursesByInstructorIdQuery, PagedResult<CourseResponse>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCoursesByInstructorIdQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CourseResponse>> Handle(GetCoursesByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            var (courses, totalCount) = await _courseRepository.GetByInstructorIdAsync(
                request.InstructorId,
                request.PageNumber,
                request.PageSize);

            var courseResponses = _mapper.Map<List<CourseResponse>>(courses);

            return new PagedResult<CourseResponse>(courseResponses, totalCount, request.PageNumber, request.PageSize);
        }
    }
} 