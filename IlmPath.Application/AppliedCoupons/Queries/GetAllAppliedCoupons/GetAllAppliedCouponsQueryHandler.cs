using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.AppliedCoupons.Queries.GetAllAppliedCoupons;
 
public class GetAllAppliedCouponsQueryHandler : IRequestHandler<GetAllAppliedCouponsQuery, (IEnumerable<AppliedCoupon>, int count)>
{
    private readonly IAppliedCouponRepository _appliedCouponRepository;

    public GetAllAppliedCouponsQueryHandler(IAppliedCouponRepository appliedCouponRepository)
    {
        _appliedCouponRepository = appliedCouponRepository;
    }

    public async Task<(IEnumerable<AppliedCoupon>, int count)> Handle(GetAllAppliedCouponsQuery request, CancellationToken cancellationToken)
    {
        var (appliedCoupons, totalCount) = await _appliedCouponRepository.GetAllAppliedCouponsAsync(request.PageNumber, request.PageSize);
        return (appliedCoupons, totalCount);
    }
}
