using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler(ICourseRepository _courseRepository):IRequestHandler<DeleteCourseCommand,Unit>
    {
      

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            
            await _courseRepository.DeleteAsync(request.Id);

            return Unit.Value;
        }
    }
}
