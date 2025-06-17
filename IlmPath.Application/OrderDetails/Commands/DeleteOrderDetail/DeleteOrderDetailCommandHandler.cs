using IlmPath.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.OrderDetails.Commands.DeleteOrderDetail;

public class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, bool>
{
    private readonly IOrderDetailRepository _orderDetailRepository;

    public DeleteOrderDetailCommandHandler(IOrderDetailRepository orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<bool> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _orderDetailRepository.DeleteOrderDetailAsync(request.id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
