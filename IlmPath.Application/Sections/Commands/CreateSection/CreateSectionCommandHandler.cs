using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Sections.DTOs;
using IlmPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Sections.Commands.CreateSection;

public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, SectionResponse>
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public CreateSectionCommandHandler(ISectionRepository sectionRepository, IMapper mapper)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }

    public async Task<SectionResponse> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        var section = new Section
        {
            CourseId = request.CourseId,
            Title = request.Title,
            Order = request.Order
        };

        await _sectionRepository.AddAsync(section);

        return _mapper.Map<SectionResponse>(section);
    }
} 