using IlmPath.Domain.Entities;

namespace IlmPath.Application.Common.Interfaces;

public class StripeConnectAccountDetails
{
    public string AccountId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool ChargesEnabled { get; set; }
    public bool PayoutsEnabled { get; set; }
    public string OnboardingUrl { get; set; } = string.Empty;
}

public class StripePayoutResult
{
    public bool Success { get; set; }
    public string? PayoutId { get; set; }
    public string? ErrorMessage { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public interface IStripeConnectService
{
    /// <summary>
    /// Creates a Stripe Express connected account for a teacher
    /// </summary>
    Task<StripeConnectAccountDetails> CreateConnectedAccountAsync(string teacherId, string email, string returnUrl, string refreshUrl);
    
    /// <summary>
    /// Gets the status and details of a connected account
    /// </summary>
    Task<StripeConnectAccountDetails> GetConnectedAccountAsync(string accountId);
    
    /// <summary>
    /// Creates an account link for onboarding or re-authentication
    /// </summary>
    Task<string> CreateAccountLinkAsync(string accountId, string returnUrl, string refreshUrl);
    
    /// <summary>
    /// Sends a payout to a connected account
    /// </summary>
    Task<StripePayoutResult> SendPayoutAsync(string connectedAccountId, decimal amount, string currency = "usd", string description = "Course earnings payout");
    
    /// <summary>
    /// Gets the payout status
    /// </summary>
    Task<StripePayoutResult> GetPayoutStatusAsync(string payoutId);
    
    /// <summary>
    /// Validates if a connected account can receive payouts
    /// </summary>
    Task<bool> CanReceivePayoutsAsync(string connectedAccountId);
    
    /// <summary>
    /// Creates a login link for Express dashboard access
    /// </summary>
    Task<string> CreateExpressDashboardLinkAsync(string connectedAccountId);
} 