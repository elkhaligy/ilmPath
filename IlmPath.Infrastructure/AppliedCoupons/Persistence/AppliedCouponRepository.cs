using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using IlmPath.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.AppliedCoupons.Persistence;

class AppliedCouponRepository : IAppliedCouponRepository
{
    private readonly ApplicationDbContext _context;


    public AppliedCouponRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAppliedCouponAsync(AppliedCoupon appliedCoupon)
    {
        await _context.AppliedCoupons.AddAsync(appliedCoupon);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAppliedCouponAsync(int id)
    {
        var appliedCoupon = await _context.AppliedCoupons.FindAsync(id);
        if (appliedCoupon == null)
            throw new NotFoundException(nameof(AppliedCoupon), id);

        _context.AppliedCoupons.Remove(appliedCoupon);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<AppliedCoupon> appliedCoupons, int TotalCount)> GetAllAppliedCouponsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.AppliedCoupons.CountAsync();

        var appliedCoupons = await _context.AppliedCoupons
        .Include(a => a.Coupon)
        .Include(a => a.Payment)
        .Include(a => a.User)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (appliedCoupons, totalCount);
    }

    public async Task<AppliedCoupon?> GetAppliedCouponByIdAsync(int id)
    {
        return await _context.AppliedCoupons
        .Include(a => a.Coupon)
        .Include(a => a.Payment)
        .Include(a => a.User)
        .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task UpdateAppliedCouponAsync(AppliedCoupon appliedCoupon)
    {
        var existingAppliedCoupon = await _context.AppliedCoupons.FindAsync(appliedCoupon.Id);
        if (existingAppliedCoupon == null)
            throw new NotFoundException(nameof(AppliedCoupon), appliedCoupon.Id);

        existingAppliedCoupon.CouponId = appliedCoupon.CouponId;
        existingAppliedCoupon.UserId = appliedCoupon.UserId;
        existingAppliedCoupon.PaymentId = appliedCoupon.PaymentId;
        existingAppliedCoupon.DiscountAmountApplied = appliedCoupon.DiscountAmountApplied;
        existingAppliedCoupon.AppliedAt = appliedCoupon.AppliedAt;


        await _context.SaveChangesAsync();
    }
}
