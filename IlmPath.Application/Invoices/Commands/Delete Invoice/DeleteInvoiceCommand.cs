using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Commands.Delete_Invoice;

public record DeleteInvoiceCommand(int id) : IRequest<bool>;
