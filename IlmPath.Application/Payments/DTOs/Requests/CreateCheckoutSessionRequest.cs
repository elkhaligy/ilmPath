namespace IlmPath.Application.Payments.DTOs.Requests;

public class CreateCheckoutSessionRequest
{
    public required string SuccessUrl { get; set; }
    public required string CancelUrl { get; set; }
} 