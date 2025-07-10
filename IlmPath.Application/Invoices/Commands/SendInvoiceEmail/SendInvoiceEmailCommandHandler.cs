using IlmPath.Application.Common.Exceptions;
using IlmPath.Application.Common.Interfaces;
using IlmPath.Application.Email;
using IlmPath.Application.Invoices.Queries.GenerateInvoiceHtml;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Commands.SendInvoiceEmail
{
    public class SendInvoiceEmailCommandHandler : IRequestHandler<SendInvoiceEmailCommand>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;

        public SendInvoiceEmailCommandHandler(IInvoiceRepository invoiceRepository, IEmailService emailService, IMediator mediator)
        {
            _invoiceRepository = invoiceRepository;
            _emailService = emailService;
            _mediator = mediator;
        }

        public async Task Handle(SendInvoiceEmailCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId);

            if (invoice?.User == null)
            {
                throw new NotFoundException($"Invoice or associated user not found for Invoice ID {request.InvoiceId}.");
            }

            var subject = $"Your IlmPath Invoice ({invoice.InvoiceNumber})";
            var body = await _mediator.Send(new GenerateInvoiceHtmlQuery { InvoiceId = request.InvoiceId }, cancellationToken);

            await _emailService.SendEmailAsync(invoice.User.Email, subject, body);

            
        }
    }
} 