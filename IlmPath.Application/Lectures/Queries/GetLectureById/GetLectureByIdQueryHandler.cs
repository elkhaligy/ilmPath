using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Lectures.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.Queries.GetLectureById
{
    public class GetLectureByIdQueryHandler : IRequestHandler<GetLectureByIdQuery, LectureResponse>
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IMapper _mapper;

        public GetLectureByIdQueryHandler(ILectureRepository lectureRepository, IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<LectureResponse> Handle(GetLectureByIdQuery request, CancellationToken cancellationToken)
        {
            var lecture = await _lectureRepository.GetByIdAsync(request.Id);

            if (lecture == null)
            {
                throw new NotFoundException(nameof(Lecture), request.Id);
            }

            return _mapper.Map<LectureResponse>(lecture);
        }
    }
} 