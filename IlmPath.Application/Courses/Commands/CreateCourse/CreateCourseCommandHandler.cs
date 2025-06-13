using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Courses.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler (ICourseRepository _courseRepository,IMapper _mapper) : IRequestHandler<CreateCourseCommand, CourseResponse>
    {




        public async Task<CourseResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = _mapper.Map<Course>(request);
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;
            
            var newCourse= await _courseRepository.AddAsync(course);

            return _mapper.Map<CourseResponse>(newCourse);




        }
    }
}
