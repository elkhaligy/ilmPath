using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler(ICourseRepository _courseRepository,IMapper _mapper) :IRequestHandler<UpdateCourseCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var courseToUpdate = await _courseRepository.GetByIdAsync(request.Id);

            if (courseToUpdate == null)
            {
               
                return Unit.Value;
            }

            _mapper.Map(request, courseToUpdate);
            courseToUpdate.UpdatedAt = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(courseToUpdate);

            return Unit.Value;
        }

    }
}
