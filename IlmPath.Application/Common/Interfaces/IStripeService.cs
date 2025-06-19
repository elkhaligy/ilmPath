using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Interfaces;

public class StripeSessionDetails
{
    public string SessionId { get; set; }
    public string PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public List<int> CourseIds { get; set; } = new();
    public string PaymentIntentId { get; set; }
}

public interface IStripeService
{
    Task<(string, string)> CreateCheckoutSessionAsync(List<CartItem> cartItems, string userId, string successUrl, string cancelUrl, string currency = "usd");
    Task<string> GetSessionStatusAsync(string sessionId);
    Task<bool> IsSessionSuccessfulAsync(string sessionId);
    Task<StripeSessionDetails?> GetSessionDetailsAsync(string sessionId);
} 