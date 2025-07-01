using AutoMapper;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Commands.CreateAppliedCoupon;
 
public class CreateAppliedCouponCommandHandler : IRequestHandler<CreateAppliedCouponCommand, AppliedCoupon>
{
    private readonly IAppliedCouponRepository _appliedCouponRepository;
    private readonly IMapper _mapper;

    public CreateAppliedCouponCommandHandler(IAppliedCouponRepository appliedCouponRepository, IMapper mapper)
    {
        _appliedCouponRepository = appliedCouponRepository;
        _mapper = mapper;
    }
    public async Task<AppliedCoupon> Handle(CreateAppliedCouponCommand request, CancellationToken cancellationToken)
    {
        // Create appliedCoupon
        var appliedCoupon = _mapper.Map<AppliedCoupon>(request);

        // Add it to the db
        await _appliedCouponRepository.AddAppliedCouponAsync(appliedCoupon);
        
        // Return appliedCoupon

        return appliedCoupon;
    }
}
