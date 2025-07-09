using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace IlmPath.Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly string _secretKey;

    public StripeService(IConfiguration configuration)
    {
        _secretKey = configuration["Stripe:SecretKey"] ?? throw new ArgumentException("Stripe secret key not configured");
        StripeConfiguration.ApiKey = _secretKey;
    }

    // Here you already have the cart items, so you don't need to get them from the database
    public async Task<(string, string)> CreateCheckoutSessionAsync(List<CartItem> cartItems, string userId, string successUrl, string cancelUrl, string currency = "usd")
    {
        // Check if cart items are null or empty
        if (cartItems == null || !cartItems.Any())
        {
            throw new ArgumentException("Cart items cannot be null or empty");
        }

        // Create line items for each cart item, using select to iterate over the cart items
        var lineItems = cartItems.Select(item => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(item.Price * 100), // Convert to cents
                Currency = currency,
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.Title,
                    Images = !string.IsNullOrEmpty(item.ThumbnailImageUrl) 
                        ? new List<string> { item.ThumbnailImageUrl } 
                        : null,
                    Metadata = new Dictionary<string, string>
                    {
                        { "courseId", item.CourseId.ToString() }
                    }
                }
            },
            Quantity = 1
        }).ToList();

        // Create a new session with the line items
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = $"{successUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId },
                { "courseIds", string.Join(",", cartItems.Select(c => c.CourseId)) }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return (session.Url, session.Id); // Return the checkout URL
    }

    public async Task<string> GetSessionStatusAsync(string sessionId)
    {
        var service = new SessionService();
        var session = await service.GetAsync(sessionId);
        
        return session.PaymentStatus ?? "unknown";
    }

    public async Task<bool> IsSessionSuccessfulAsync(string sessionId)
    {
        var service = new SessionService();
        var session = await service.GetAsync(sessionId);
        
        return session.PaymentStatus == "paid";
    }

    public async Task<StripeSessionDetails?> GetSessionDetailsAsync(string sessionId)
    {
        try
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (session?.Metadata == null)
                return null;

            // Extract course IDs from metadata
            var courseIdsString = session.Metadata.GetValueOrDefault("courseIds");
            var courseIds = new List<int>();
            
            if (!string.IsNullOrEmpty(courseIdsString))
            {
                courseIds = courseIdsString
                    .Split(',')
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToList();
            }

            return new StripeSessionDetails
            {
                SessionId = session.Id,
                PaymentStatus = session.PaymentStatus ?? "unknown",
                TotalAmount = (decimal)(session.AmountTotal ?? 0) / 100, // Convert from cents
                CourseIds = courseIds,
                PaymentIntentId = session.PaymentIntentId ?? session.Id,
                SuccessUrl = session.SuccessUrl ?? string.Empty
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
} 