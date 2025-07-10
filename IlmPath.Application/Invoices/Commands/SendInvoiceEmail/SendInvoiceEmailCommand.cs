using MediatR;

namespace IlmPath.Application.Invoices.Commands.SendInvoiceEmail
{
    public class SendInvoiceEmailCommand : IRequest
    {
        public int InvoiceId { get; set; }
    }
} 