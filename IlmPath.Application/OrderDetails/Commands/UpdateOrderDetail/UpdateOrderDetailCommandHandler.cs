using AutoMapper;
using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Commands.UpdateOrderDetail;

public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, OrderDetail>
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IMapper _mapper;


    public UpdateOrderDetailCommandHandler(IOrderDetailRepository orderDetailRepository, IMapper mapper)
    {
        _orderDetailRepository = orderDetailRepository;
        _mapper = mapper;
    }

    public async Task<OrderDetail> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var orderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(request.Id);

        if (orderDetail == null)
        {
            throw new NotFoundException(nameof(OrderDetail), request.Id);
        }

        orderDetail = _mapper.Map<OrderDetail>(request);

        await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);

        return orderDetail;
    }
}
