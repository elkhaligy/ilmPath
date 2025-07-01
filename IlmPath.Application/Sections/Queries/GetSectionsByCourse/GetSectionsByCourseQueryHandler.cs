using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Sections.DTOs;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Sections.Queries.GetSectionsByCourse;

public class GetSectionsByCourseQueryHandler : IRequestHandler<GetSectionsByCourseQuery, IEnumerable<SectionResponse>>
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public GetSectionsByCourseQueryHandler(ISectionRepository sectionRepository, IMapper mapper)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SectionResponse>> Handle(GetSectionsByCourseQuery request, CancellationToken cancellationToken)
    {
        var sections = await _sectionRepository.GetByCourseIdAsync(request.CourseId);
        return _mapper.Map<IEnumerable<SectionResponse>>(sections);
    }
} 