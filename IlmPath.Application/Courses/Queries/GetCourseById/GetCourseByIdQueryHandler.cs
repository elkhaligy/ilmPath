using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Queries.GetCourseById
{
    public class GetCourseByIdQueryHandler (ICourseRepository _courseRepository , IMapper _mapper) : IRequestHandler<GetCourseByIdQuery, CourseResponse>
    {
        public async Task<CourseResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {

            var course = await _courseRepository.GetByIdAsync(request.Id);


            // Mafrod n handle el errors hna 

            if(course == null)
            {
                throw new  NotFoundException(nameof(Course),request.Id);
            }

            return _mapper.Map<CourseResponse>(course);



        }
    }
}
