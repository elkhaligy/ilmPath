using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.InvoiceItems.Commands.DeleteInvoiceItem;
public record DeleteInvoiceItemCommand(int id) : IRequest<bool>;

