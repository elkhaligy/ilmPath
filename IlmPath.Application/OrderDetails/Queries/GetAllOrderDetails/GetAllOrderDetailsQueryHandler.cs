using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Queries.GetAllOrderDetails;

public class GetAllOrderDetailsQueryHandler : IRequestHandler<GetAllOrderDetailsQuery, (IEnumerable<OrderDetail>, int count)>
{
    private readonly IOrderDetailRepository _orderDetailRepository;

    public GetAllOrderDetailsQueryHandler(IOrderDetailRepository orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<(IEnumerable<OrderDetail>, int count)> Handle(GetAllOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var (orderDetails, totalCount) = await _orderDetailRepository.GetAllOrderDetailsAsync(request.PageNumber, request.PageSize);
        return (orderDetails, totalCount);
    }
}