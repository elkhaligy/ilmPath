using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Commands.UpdateInvoiceItem;
 
public record UpdateInvoiceItemCommand(int Id, int InvoiceId, int CourseId, string Description, decimal OriginalUnitPrice, decimal DiscountAppliedOnItem, decimal UnitPrice) : IRequest<InvoiceItem>;

