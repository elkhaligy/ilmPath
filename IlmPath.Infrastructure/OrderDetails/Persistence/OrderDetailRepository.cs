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

namespace IlmPath.Infrastructure.OrderDetails.Persistence;

class OrderDetailRepository : IOrderDetailRepository
{
    private readonly ApplicationDbContext _context;

    public OrderDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddOrderDetailAsync(OrderDetail orderDetail)
    {
        await _context.OrderDetails.AddAsync(orderDetail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderDetailAsync(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null)
            throw new NotFoundException(nameof(OrderDetail), id);

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<OrderDetail> orderDetails, int TotalCount)> GetAllOrderDetailsAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.OrderDetails.CountAsync();

        var orderDetails = await _context.OrderDetails
        .Include(o => o.Course)
        .Include(o => o.Enrollment)
        .Include(o => o.Payment)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return (orderDetails, totalCount);
    }

    public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
    {
        return await _context.OrderDetails
                  .Include(o => o.Course)
                  .Include(o => o.Enrollment)
                  .Include(o => o.Payment)
                  .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
        var existingOrderDetail = await _context.OrderDetails.FindAsync(orderDetail.Id);
        if (existingOrderDetail == null)
            throw new NotFoundException(nameof(OrderDetail), orderDetail.Id);

        existingOrderDetail.PaymentId = orderDetail.PaymentId;
        existingOrderDetail.EnrollmentId = orderDetail.EnrollmentId;
        existingOrderDetail.CourseId = orderDetail.CourseId;
        existingOrderDetail.PriceAtPurchase = orderDetail.PriceAtPurchase;


        await _context.SaveChangesAsync();
    }
}
