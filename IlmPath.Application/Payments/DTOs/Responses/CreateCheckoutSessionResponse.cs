namespace IlmPath.Application.Payments.DTOs.Responses;

public class CreateCheckoutSessionResponse
{
    public required string CheckoutUrl { get; set; }
    public required string SessionId { get; set; }
} 