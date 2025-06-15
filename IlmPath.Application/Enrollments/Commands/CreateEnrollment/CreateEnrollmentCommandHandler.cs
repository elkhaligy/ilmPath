using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Enrollments.Commands.CreateEnrollment;

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, Enrollment>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;

    public CreateEnrollmentCommandHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
    }
    public async Task<Enrollment> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        // Create enrollment
        var enrollment = _mapper.Map<Enrollment>(request);

        // Add it to the db
        await _enrollmentRepository.AddEnrollmentAsync(enrollment);
       
        // Return enrollment
        return enrollment;
    }
    
}
