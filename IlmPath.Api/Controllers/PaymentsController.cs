using IlmPath.Application.Payments.Commands.CreateCheckoutSession;
using IlmPath.Application.Payments.Commands.ProcessPaymentSuccess;
using IlmPath.Application.Payments.DTOs.Requests;
using IlmPath.Application.Payments.DTOs.Responses;
using IlmPath.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers;

// Endpoint for payments
// POST /api/payments/checkout-session
// POST /api/payments/verify-payment/{sessionId}
// GET /api/payments/my-payments

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentsController(IMediator _mediator, IStripeService _stripeService, IPaymentRepository _paymentRepository) : ControllerBase
{
    private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost("checkout-session")]
    public async Task<ActionResult<CreateCheckoutSessionResponse>> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
    {
        var userId = GetCurrentUserId();
        var command = new CreateCheckoutSessionCommand(userId, request.SuccessUrl, request.CancelUrl);
        
        var (checkoutUrl, sessionId) = await _mediator.Send(command);
        
        var response = new CreateCheckoutSessionResponse { CheckoutUrl = checkoutUrl, SessionId = sessionId };
        return Ok(response);
    }

    // This endpoint is used to verify if the payment was successful and process the payment
    // So the command will be called ProcessPaymentSuccessCommand
    [HttpPost("verify-payment/{sessionId}")]
    public async Task<ActionResult<VerifyPaymentResponse>> VerifyAndProcessPayment(string sessionId)
    {
        var userId = GetCurrentUserId();
        
        // Check if payment was successful
        var isSuccessful = await _stripeService.IsSessionSuccessfulAsync(sessionId);
        
        if (!isSuccessful)
        {
            return Ok(new VerifyPaymentResponse 
            { 
                Success = false, 
                Message = "Payment not completed yet" 
            });
        }

        // Check if already processed (prevent duplicate processing)
        var existingPayment = await _paymentRepository.GetPaymentByTransactionIdAsync(sessionId);
        if (existingPayment != null)
        {
            return Ok(new VerifyPaymentResponse 
            { 
                Success = true, 
                Message = "Payment already processed", 
                AlreadyProcessed = true,
                SessionId = sessionId
            });
        }

        // Process the payment
        var command = new ProcessPaymentSuccessCommand(sessionId, userId);
        var (success, successUrl) = await _mediator.Send(command);
        
        if (success)
        {
            return Ok(new VerifyPaymentResponse 
            { 
                Success = true, 
                Message = "Payment processed successfully! You are now enrolled in your courses.",
                SuccessUrl = successUrl,
                SessionId = sessionId
            });
        }
        
        return StatusCode(500, new VerifyPaymentResponse 
        { 
            Success = false, 
            Message = "Error processing payment" 
        });
    }

    [HttpGet("my-payments")]
    public async Task<ActionResult> GetMyPayments()
    {
        var userId = GetCurrentUserId();
        var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
        
        return Ok(payments);
    }
} 