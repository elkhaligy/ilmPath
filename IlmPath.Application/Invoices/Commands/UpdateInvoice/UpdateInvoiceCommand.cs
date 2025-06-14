using IlmPath.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Invoices.Commands.UpdateInvoice; 
public record UpdateInvoiceCommand(int Id, string InvoiceNumber, string UserId, int PaymentId, DateTime IssueDate, DateTime? DueDate, decimal TotalAmount, string? BillingAddress, string Status, string? Notes) : IRequest<Invoice>;

