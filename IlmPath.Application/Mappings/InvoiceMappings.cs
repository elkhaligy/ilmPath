using AutoMapper;
using IlmPath.Application.Invoices.Commands.CreateInvoice;
using IlmPath.Application.Invoices.Commands.UpdateInvoice;
using IlmPath.Application.Invoices.DTOs.Requests;
using IlmPath.Application.Invoices.DTOs.Responses;
using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Mappings;

class InvoiceMappings : Profile
{
    public InvoiceMappings()
    {
        // Domain to Response DTO
        CreateMap<Invoice, InvoiceResponse>();
        CreateMap<CreateInvoiceCommand, Enrollment>();


        // Request DTO to Command
        CreateMap<CreateInvoiceRequest, CreateInvoiceCommand>();

        // For UpdateCategoryCommand, we need to handle the Id parameter
        //CreateMap<(UpdateInvoiceRequest Request, int Id), UpdateInvoiceCommand>()
        //    .ConstructUsing(src => new UpdateInvoiceCommand(src.Id, src.Request.EnrollmentDate, src.Request.PricePaid));
    }
}
