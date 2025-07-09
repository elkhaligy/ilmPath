namespace IlmPath.Application.Payments.DTOs.Responses;

public class VerifyPaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool AlreadyProcessed { get; set; }
    public string? SuccessUrl { get; set; }
    public string? SessionId { get; set; }
} 