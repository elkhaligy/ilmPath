using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface IAppliedCouponRepository
    {
        Task<AppliedCoupon?> GetAppliedCouponByIdAsync(int id);
        Task<(IEnumerable<AppliedCoupon> appliedCoupons, int TotalCount)> GetAllAppliedCouponsAsync(int pageNumber, int pageSize);
        Task AddAppliedCouponAsync(AppliedCoupon appliedCoupon);
        Task UpdateAppliedCouponAsync(AppliedCoupon appliedCoupon);
        Task DeleteAppliedCouponAsync(int id);
    }
}
