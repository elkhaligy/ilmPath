using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.Commands.UpdateLecture
{
    public class UpdateLectureCommandHandler : IRequestHandler<UpdateLectureCommand, Unit>
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IMapper _mapper;

        public UpdateLectureCommandHandler(ILectureRepository lectureRepository, IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLectureCommand request, CancellationToken cancellationToken)
        {
            var lectureToUpdate = await _lectureRepository.GetByIdAsync(request.Id);

            if (lectureToUpdate == null)
            {
                throw new NotFoundException(nameof(Lecture), request.Id);
            }

            _mapper.Map(request, lectureToUpdate);

            await _lectureRepository.UpdateAsync(lectureToUpdate);

            return Unit.Value;
        }
    }
} 