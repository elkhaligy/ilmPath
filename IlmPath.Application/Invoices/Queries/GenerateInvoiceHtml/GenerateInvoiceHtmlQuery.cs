using MediatR;

namespace IlmPath.Application.Invoices.Queries.GenerateInvoiceHtml
{
    public class GenerateInvoiceHtmlQuery : IRequest<string>
    {
        public int InvoiceId { get; set; }
    }
} 