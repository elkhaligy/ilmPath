using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Sections.Commands.DeleteSection;

public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand>
{
    private readonly ISectionRepository _sectionRepository;

    public DeleteSectionCommandHandler(ISectionRepository sectionRepository)
    {
        _sectionRepository = sectionRepository;
    }

    public async Task Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await _sectionRepository.GetByIdAsync(request.Id);

        if (section == null)
        {
            throw new NotFoundException(nameof(Section), request.Id);
        }

        await _sectionRepository.DeleteAsync(request.Id);
    }
} 