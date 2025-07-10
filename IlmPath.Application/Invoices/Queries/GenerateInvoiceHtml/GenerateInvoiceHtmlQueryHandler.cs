using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Queries.GenerateInvoiceHtml
{
    public class GenerateInvoiceHtmlQueryHandler : IRequestHandler<GenerateInvoiceHtmlQuery, string>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GenerateInvoiceHtmlQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<string> Handle(GenerateInvoiceHtmlQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId);

            if (invoice == null)
            {
                throw new NotFoundException($"Invoice with ID {request.InvoiceId} not found.");
            }

            var htmlBuilder = new StringBuilder();

            // Basic HTML structure and styling
            htmlBuilder.Append(@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Invoice</title>
                    <style>
                        body { font-family: Arial, sans-serif; color: #333; }
                        .container { max-width: 800px; margin: 20px auto; padding: 20px; border: 1px solid #eee; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
                        .header { text-align: center; padding-bottom: 20px; border-bottom: 1px solid #eee; }
                        .header h1 { margin: 0; color: #4CAF50; }
                        .invoice-details { margin: 20px 0; }
                        .invoice-details, .billing-details { line-height: 1.6; }
                        .invoice-details table, .items-table { width: 100%; }
                        .items-table { border-collapse: collapse; margin-top: 20px; }
                        .items-table th, .items-table td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                        .items-table th { background-color: #f2f2f2; }
                        .total { text-align: right; margin-top: 20px; font-size: 1.2em; }
                        .footer { text-align: center; margin-top: 20px; font-size: 0.8em; color: #777; }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>IlmPath Invoice</h1>
                        </div>
                        <div class='invoice-details'>
                            <table>
                                <tr>
                                    <td><strong>Invoice Number:</strong></td>
                                    <td>" + invoice.InvoiceNumber + @"</td>
                                </tr>
                                <tr>
                                    <td><strong>Issue Date:</strong></td>
                                    <td>" + invoice.IssueDate.ToString("MMMM dd, yyyy") + @"</td>
                                </tr>
                            </table>
                        </div>
                        <div class='billing-details'>
                            <strong>Billed To:</strong><br>" +
                            invoice.User.FirstName + " " + invoice.User.LastName + @"<br>" +
                            invoice.User.Email + @"
                        </div>
                        <table class='items-table'>
                            <thead>
                                <tr>
                                    <th>Course</th>
                                    <th>Description</th>
                                    <th>Price</th>
                                </tr>
                            </thead>
                            <tbody>");

            foreach (var item in invoice.Items)
            {
                htmlBuilder.Append($@"
                    <tr>
                        <td>{item.Course?.Title}</td>
                        <td>{item.Description}</td>
                        <td>${item.UnitPrice:0.00}</td>
                    </tr>");
            }

            htmlBuilder.Append(@"
                            </tbody>
                        </table>
                        <div class='total'>
                            <strong>Total: $" + invoice.TotalAmount.ToString("0.00") + @"</strong>
                        </div>
                        <div class='footer'>
                            <p>Thank you for your business!</p>
                            <p>IlmPath | 123 Learning Lane, Knowledge City, 45678</p>
                        </div>
                    </div>
                </body>
                </html>");

            return htmlBuilder.ToString();
        }
    }
} 