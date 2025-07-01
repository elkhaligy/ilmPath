using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces;

public interface IOrderDetailRepository
{
    Task<OrderDetail?> GetOrderDetailByIdAsync(int id);
    Task<(IEnumerable<OrderDetail> orderDetails, int TotalCount)> GetAllOrderDetailsAsync(int pageNumber, int pageSize);
    Task AddOrderDetailAsync(OrderDetail orderDetail);
    Task UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task DeleteOrderDetailAsync(int id);
}
