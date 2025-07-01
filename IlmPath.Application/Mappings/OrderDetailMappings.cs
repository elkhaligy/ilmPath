using AutoMapper;
using IlmPath.Application.Invoices.Commands.UpdateInvoice;
using IlmPath.Application.OrderDetails.Commands.CreateOrderDetail;
using IlmPath.Application.OrderDetails.DTOs.Requests;
using IlmPath.Application.OrderDetails.DTOs.Responses;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IlmPath.Application.OrderDetails.Commands.UpdateOrderDetail;

namespace IlmPath.Application.Mappings;

class OrderDetailMappings : Profile
{
    public OrderDetailMappings()
    {
        // Domain to Response DTO
        CreateMap<OrderDetail, OrderDetailResponse>();

        //command to Domain
        CreateMap<CreateOrderDetailCommand, OrderDetail>();
        CreateMap<UpdateOrderDetailCommand, OrderDetail>();




        // Request DTO to Command
        CreateMap<CreateOrderDetailRequest, CreateOrderDetailCommand>();

        // For UpdateOrderDetailCommand, we need to handle the Id parameter
        CreateMap<(UpdateOrderDetailRequest Request, int Id), UpdateOrderDetailCommand>()
            .ConstructUsing(src => new UpdateOrderDetailCommand(src.Id, src.Request.PaymentId, src.Request.EnrollmentId, src.Request.CourseId, src.Request.PriceAtPurchase));
    }
}