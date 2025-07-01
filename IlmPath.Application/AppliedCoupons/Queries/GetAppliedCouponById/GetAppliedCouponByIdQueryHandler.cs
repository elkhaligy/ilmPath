using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Queries.GetAppliedCouponById;
 
public class GetAppliedCouponByIdQueryHandler : IRequestHandler<GetAppliedCouponByIdQuery, AppliedCoupon>
{
    private readonly IAppliedCouponRepository _appliedCouponRepository;

    public GetAppliedCouponByIdQueryHandler(IAppliedCouponRepository appliedCouponRepository)
    {
        _appliedCouponRepository = appliedCouponRepository;
    }

    public async Task<AppliedCoupon> Handle(GetAppliedCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var appliedCoupon = await _appliedCouponRepository.GetAppliedCouponByIdAsync(request.Id);

        if (appliedCoupon == null)
        {
            throw new NotFoundException(nameof(AppliedCoupon), request.Id);
        }

        return appliedCoupon;
    }
}


