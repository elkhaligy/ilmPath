using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Lectures.DTOs;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.Queries.GetLecturesBySection
{
    public class GetLecturesBySectionQueryHandler : IRequestHandler<GetLecturesBySectionQuery, IEnumerable<LectureResponse>>
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IMapper _mapper;

        public GetLecturesBySectionQueryHandler(ILectureRepository lectureRepository, IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LectureResponse>> Handle(GetLecturesBySectionQuery request, CancellationToken cancellationToken)
        {
            var lectures = await _lectureRepository.GetBySectionIdAsync(request.SectionId);
            return _mapper.Map<IEnumerable<LectureResponse>>(lectures);
        }
    }
} 