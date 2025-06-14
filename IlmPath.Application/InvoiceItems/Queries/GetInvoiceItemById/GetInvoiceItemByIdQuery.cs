using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Queries.GetInvoiceItemById;

public record GetInvoiceItemByIdQuery(int Id) : IRequest<InvoiceItem>;
