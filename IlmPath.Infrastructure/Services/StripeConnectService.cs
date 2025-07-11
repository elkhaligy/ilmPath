using IlmPath.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace IlmPath.Infrastructure.Services;

public class StripeConnectService : IStripeConnectService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<StripeConnectService> _logger;

    public StripeConnectService(IConfiguration configuration, ILogger<StripeConnectService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"] 
            ?? throw new InvalidOperationException("Stripe SecretKey not configured");
    }

    public async Task<StripeConnectAccountDetails> CreateConnectedAccountAsync(string teacherId, string email, string returnUrl, string refreshUrl)
    {
        var options = new AccountCreateOptions
        {
            Type = "express",
            Country = "US", // You can make this configurable
            Email = email,
            Capabilities = new AccountCapabilitiesOptions
            {
                CardPayments = new AccountCapabilitiesCardPaymentsOptions
                {
                    Requested = true,
                },
                Transfers = new AccountCapabilitiesTransfersOptions
                {
                    Requested = true,
                },
            },
            Metadata = new Dictionary<string, string>
            {
                { "teacher_id", teacherId }
            }
        };

        var service = new AccountService();
        var account = await service.CreateAsync(options);

        // Create onboarding link
        var onboardingUrl = await CreateAccountLinkAsync(account.Id, returnUrl, refreshUrl);

        return new StripeConnectAccountDetails
        {
            AccountId = account.Id,
            Status = account.DetailsSubmitted ? "active" : "pending",
            ChargesEnabled = account.ChargesEnabled,
            PayoutsEnabled = account.PayoutsEnabled,
            OnboardingUrl = onboardingUrl
        };
    }

    public async Task<StripeConnectAccountDetails> GetConnectedAccountAsync(string accountId)
    {
        try
        {
            var service = new AccountService();
            var account = await service.GetAsync(accountId);

            return new StripeConnectAccountDetails
            {
                AccountId = account.Id,
                Status = account.DetailsSubmitted ? "active" : "pending",
                ChargesEnabled = account.ChargesEnabled,
                PayoutsEnabled = account.PayoutsEnabled,
                OnboardingUrl = string.Empty // Will be set separately if needed
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get Stripe connected account {AccountId}", accountId);
            throw;
        }
    }

    public async Task<string> CreateAccountLinkAsync(string accountId, string returnUrl, string refreshUrl)
    {
        try
        {
            var options = new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding",
            };

            var service = new AccountLinkService();
            var accountLink = await service.CreateAsync(options);

            return accountLink.Url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create account link for account {AccountId}", accountId);
            throw;
        }
    }

    public async Task<StripePayoutResult> SendPayoutAsync(string connectedAccountId, decimal amount, string currency = "usd", string description = "Course earnings payout")
    {
        try
        {


            var transferService = new TransferService();
            var transfer = await transferService.CreateAsync(new TransferCreateOptions
            {
                Amount = (long)(amount * 100),        // amount in cents
                Currency = "usd",
                Destination = connectedAccountId      // e.g. acct_1RjY8MQfkmmdlRw0
            });
  
            return new StripePayoutResult
            {
                Success = true,
                PayoutId = transfer.Id,
                Amount = amount,
                Status = null,
                ErrorMessage = null
            };
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe payout failed for account {AccountId}. Error: {Error}", 
                connectedAccountId, ex.Message);
            
            return new StripePayoutResult
            {
                Success = false,
                PayoutId = null,
                Amount = amount,
                Status = "failed",
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending Stripe payout to account {AccountId}", connectedAccountId);
            
            return new StripePayoutResult
            {
                Success = false,
                PayoutId = null,
                Amount = amount,
                Status = "failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<StripePayoutResult> GetPayoutStatusAsync(string payoutId)
    {
        try
        {
            var service = new PayoutService();
            var payout = await service.GetAsync(payoutId);

            return new StripePayoutResult
            {
                Success = payout.Status == "paid",
                PayoutId = payoutId,
                Amount = (decimal)payout.Amount / 100, // Convert from cents
                Status = payout.Status,
                ErrorMessage = payout.FailureCode
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get payout status for {PayoutId}", payoutId);
            
            return new StripePayoutResult
            {
                Success = false,
                PayoutId = payoutId,
                Amount = 0,
                Status = "unknown",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<bool> CanReceivePayoutsAsync(string connectedAccountId)
    {
        try
        {
            var account = await GetConnectedAccountAsync(connectedAccountId);
            return account.PayoutsEnabled && account.ChargesEnabled;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> CreateExpressDashboardLinkAsync(string connectedAccountId)
    {
        try
        {
            // Create login link using direct HTTP request to Stripe API
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", StripeConfiguration.ApiKey);
            
            var response = await httpClient.PostAsync(
                $"https://api.stripe.com/v1/accounts/{connectedAccountId}/login_links", 
                null);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var loginLink = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                var loginUrl = loginLink.url.ToString();
                
                _logger.LogInformation("Stripe Express dashboard link created for account {AccountId}", connectedAccountId);
                return loginUrl;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create login link: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Stripe Express dashboard link for account {AccountId}", connectedAccountId);
            throw;
        }
    }
} 