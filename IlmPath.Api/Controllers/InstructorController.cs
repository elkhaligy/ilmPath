using IlmPath.Application.Common.Pagination;
using IlmPath.Application.Payouts.Commands.GenerateInstructorPayout;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using IlmPath.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using IlmPath.Domain.Entities;
using Stripe;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class InstructorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInstructorPayoutRepository _payoutRepository;
        private readonly IStripeConnectService _stripeConnectService;
        private readonly UserManager<ApplicationUser> _userManager;

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public InstructorController(
            IMediator mediator, 
            IInstructorPayoutRepository payoutRepository,
            IStripeConnectService stripeConnectService,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _payoutRepository = payoutRepository;
            _stripeConnectService = stripeConnectService;
            _userManager = userManager;
        }

        // GET: api/instructor/balance
        [HttpGet("balance")]
        [ProducesResponseType(typeof(InstructorBalance), StatusCodes.Status200OK)]
        public async Task<ActionResult<InstructorBalance>> GetBalance()
        {
            var instructorId = GetCurrentUserId();
            var pendingBalance = await _payoutRepository.GetPendingBalanceAsync(instructorId);

            return Ok(new InstructorBalance
            {
                PendingBalance = pendingBalance,
                LastUpdated = DateTime.UtcNow
            });
        }

        // GET: api/instructor/unpaid-enrollments
        [HttpGet("unpaid-enrollments")]
        [ProducesResponseType(typeof(PagedResult<UnpaidEnrollmentSummary>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<UnpaidEnrollmentSummary>>> GetUnpaidEnrollments(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var instructorId = GetCurrentUserId();
            var unpaidEnrollments = await _payoutRepository.GetUnpaidEnrollmentsAsync(instructorId);

            var enrollmentSummaries = unpaidEnrollments.Select(e => new UnpaidEnrollmentSummary
            {
                EnrollmentId = e.Id,
                StudentName = $"{e.User?.FirstName} {e.User?.LastName}",
                CourseName = e.Course?.Title ?? "Unknown Course",
                AmountPaid = e.PricePaid,
                InstructorShare = e.PricePaid * 0.70m, // 70% goes to instructor
                EnrollmentDate = e.EnrollmentDate
            }).ToList();

            var totalCount = enrollmentSummaries.Count;
            var pagedEnrollments = enrollmentSummaries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new PagedResult<UnpaidEnrollmentSummary>(pagedEnrollments, totalCount, pageNumber, pageSize));
        }

        // GET: api/instructor/payouts
        [HttpGet("payouts")]
        [ProducesResponseType(typeof(PagedResult<InstructorPayoutSummary>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<InstructorPayoutSummary>>> GetPayouts(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var instructorId = GetCurrentUserId();
            var (payouts, totalCount) = await _payoutRepository.GetPayoutsByInstructorIdAsync(instructorId, pageNumber, pageSize);

            var payoutSummaries = payouts.Select(p => new InstructorPayoutSummary
            {
                Id = p.Id,
                GrossAmount = p.GrossAmount,
                CommissionAmount = p.CommissionAmount,
                NetAmount = p.NetAmount,
                Status = p.Status,
                PaymentMethod = p.PaymentMethod,
                RequestDate = p.PayoutDate,
                ProcessedDate = p.ProcessedDate,
                EnrollmentsCount = p.PayoutEnrollments.Count,
                Notes = p.Notes
            }).ToList();

            return Ok(new PagedResult<InstructorPayoutSummary>(payoutSummaries, totalCount, pageNumber, pageSize));
        }

        // POST: api/instructor/request-withdrawal
        [HttpPost("request-withdrawal")]
        [ProducesResponseType(typeof(InstructorPayoutSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InstructorPayoutSummary>> RequestWithdrawal(
            [FromBody] RequestWithdrawalRequest request)
        {
            var instructorId = GetCurrentUserId();

            // Check if instructor has pending balance
            var pendingBalance = await _payoutRepository.GetPendingBalanceAsync(instructorId);
            if (pendingBalance <= 0)
            {
                return BadRequest("No pending balance available for withdrawal.");
            }

            var command = new GenerateInstructorPayoutCommand
            {
                InstructorId = instructorId,
                PaymentMethod = "Stripe",
                Notes = request.Notes
            };

            var payout = await _mediator.Send(command);

            var summary = new InstructorPayoutSummary
            {
                Id = payout.Id,
                GrossAmount = payout.GrossAmount,
                CommissionAmount = payout.CommissionAmount,
                NetAmount = payout.NetAmount,
                Status = payout.Status,
                PaymentMethod = payout.PaymentMethod,
                RequestDate = payout.PayoutDate,
                ProcessedDate = payout.ProcessedDate,
                EnrollmentsCount = payout.PayoutEnrollments.Count,
                Notes = payout.Notes
            };

            return Ok(summary);
        }

        // POST: api/instructor/create-stripe-account
        [HttpPost("create-stripe-account")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStripeAccount()
        {
            var instructorId = GetCurrentUserId();
            var instructor = await _userManager.FindByIdAsync(instructorId);
            
            if (instructor == null)
            {
                return BadRequest("Instructor not found");
            }

            if (!string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return Conflict("Stripe Connect account already exists for this user.");
            }

            try
            {
                var options = new AccountCreateOptions
                {
                    Type = "express",
                    Country = "US",
                    Email = instructor.Email!,
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
                    BusinessType = "individual",
                    Metadata = new Dictionary<string, string>
                    {
                        { "teacher_id", instructorId }
                    }
                };

                var service = new AccountService();
                var account = await service.CreateAsync(options);

                // Save the account ID to the user
                instructor.StripeConnectAccountId = account.Id;
                await _userManager.UpdateAsync(instructor);

                return Ok(new { StripeAccountId = account.Id });
            }
            catch (StripeException ex)
            {
                return StatusCode(502, $"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // GET: api/instructor/stripe-onboarding-link
        [HttpGet("stripe-onboarding-link")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStripeOnboardingLink()
        {
            var instructorId = GetCurrentUserId();
            var instructor = await _userManager.FindByIdAsync(instructorId);
            
            if (instructor == null)
            {
                return NotFound("Instructor not found");
            }

            if (string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return BadRequest("Stripe account not created yet.");
            }

            try
            {
                var accountLinkService = new AccountLinkService();
                var options = new AccountLinkCreateOptions
                {
                    Account = instructor.StripeConnectAccountId,
                    RefreshUrl = $"{Request.Scheme}://{Request.Host}/teacher/payouts?setup=refresh",
                    ReturnUrl = $"{Request.Scheme}://{Request.Host}/teacher/payouts?setup=complete",
                    Type = "account_onboarding"
                };
                var accountLink = await accountLinkService.CreateAsync(options);
                return Ok(new { Url = accountLink.Url });
            }
            catch (StripeException ex)
            {
                return StatusCode(502, $"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // GET: api/instructor/stripe-dashboard
        [HttpGet("stripe-dashboard")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetStripeDashboardLink()
        {
            var instructorId = GetCurrentUserId();
            var instructor = await _userManager.FindByIdAsync(instructorId);
            
            if (instructor == null)
            {
                return BadRequest("Instructor not found");
            }

            if (string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return BadRequest("No Stripe Connect account found. Please set up your Stripe account first.");
            }

            try
            {
                var dashboardUrl = await _stripeConnectService.CreateExpressDashboardLinkAsync(instructor.StripeConnectAccountId);
                return Ok(dashboardUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create dashboard link: {ex.Message}");
            }
        }

        // GET: api/instructor/stripe-connect-status
        [HttpGet("stripe-connect-status")]
        [ProducesResponseType(typeof(StripeConnectStatusResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<StripeConnectStatusResponse>> GetStripeConnectStatus()
        {
            var instructorId = GetCurrentUserId();
            var instructor = await _userManager.FindByIdAsync(instructorId);
            
            if (instructor == null || string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return Ok(new StripeConnectStatusResponse
                {
                    IsConnected = false,
                    CanReceivePayouts = false,
                    AccountId = null,
                    Status = "Not connected"
                });
            }

            try
            {
                var accountDetails = await _stripeConnectService.GetConnectedAccountAsync(instructor.StripeConnectAccountId);
                
                return Ok(new StripeConnectStatusResponse
                {
                    IsConnected = true,
                    CanReceivePayouts = accountDetails.PayoutsEnabled && accountDetails.ChargesEnabled,
                    AccountId = instructor.StripeConnectAccountId,
                    Status = accountDetails.PayoutsEnabled ? "Active" : "Setup required"
                });
            }
            catch
            {
                return Ok(new StripeConnectStatusResponse
                {
                    IsConnected = false,
                    CanReceivePayouts = false,
                    AccountId = instructor.StripeConnectAccountId,
                    Status = "Error - account may be invalid"
                });
            }
        }

        // POST: api/instructor/withdraw/{payoutId}
        [HttpPost("withdraw/{payoutId}")]
        [ProducesResponseType(typeof(WithdrawPayoutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WithdrawPayoutResponse>> WithdrawToStripe(int payoutId)
        {
            var instructorId = GetCurrentUserId();
            
            // Get the payout request
            var payout = await _payoutRepository.GetPayoutByIdAsync(payoutId);
            if (payout == null)
            {
                return NotFound($"Payout request with ID {payoutId} not found");
            }

            // Verify this payout belongs to the current instructor
            if (payout.InstructorId != instructorId)
            {
                return BadRequest("You can only withdraw your own approved payouts");
            }

            // Check if payout is approved and not already completed
            if (payout.Status != "Approved")
            {
                return BadRequest($"Cannot withdraw payout with status '{payout.Status}'. Only approved payouts can be withdrawn.");
            }

            // Get instructor's Stripe Connect account
            var instructor = await _userManager.FindByIdAsync(instructorId);
            if (instructor == null || string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return BadRequest("Please set up your Stripe Connect account before withdrawing funds.");
            }

            // Validate Stripe account can receive payouts
            var canReceivePayouts = await _stripeConnectService.CanReceivePayoutsAsync(instructor.StripeConnectAccountId);
            if (!canReceivePayouts)
            {
                return BadRequest("Your Stripe account is not yet ready to receive payouts. Please complete your account setup.");
            }

            try
            {
                // Send payout via Stripe Connect
                var payoutResult = await _stripeConnectService.SendPayoutAsync(
                    instructor.StripeConnectAccountId,
                    payout.NetAmount,
                    "usd",
                    $"Course earnings payout - Request #{payout.Id}");

                if (payoutResult.Success)
                {
                    // Update payout status to completed
                    payout.Status = "Completed";
                    payout.ExternalTransactionId = payoutResult.PayoutId;
                    payout.ProcessedDate = DateTime.UtcNow;
                    payout.Notes = (payout.Notes ?? "") + $"\nStripe payout sent to account {instructor.StripeConnectAccountId}";

                    await _payoutRepository.UpdatePayoutAsync(payout);

                    return Ok(new WithdrawPayoutResponse
                    {
                        Success = true,
                        Message = "Payout sent successfully to your Stripe account",
                        TransactionId = payoutResult.PayoutId,
                        Amount = payout.NetAmount,
                        StripeAccountId = instructor.StripeConnectAccountId
                    });
                }
                else
                {
                    return BadRequest(new WithdrawPayoutResponse
                    {
                        Success = false,
                        Message = payoutResult.ErrorMessage ?? "Failed to send payout",
                        TransactionId = null,
                        Amount = payout.NetAmount,
                        StripeAccountId = instructor.StripeConnectAccountId
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new WithdrawPayoutResponse
                {
                    Success = false,
                    Message = "An error occurred while processing your withdrawal",
                    TransactionId = null,
                    Amount = payout.NetAmount,
                    StripeAccountId = instructor.StripeConnectAccountId
                });
            }
        }
    }

    // DTOs
    public class InstructorBalance
    {
        public decimal PendingBalance { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class InstructorPayoutSummary
    {
        public int Id { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public int EnrollmentsCount { get; set; }
        public string? Notes { get; set; }
    }

    public class RequestWithdrawalRequest
    {
        public string? Notes { get; set; }
    }

    public class UnpaidEnrollmentSummary
    {
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal InstructorShare { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class WithdrawPayoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? StripeAccountId { get; set; }
    }

    public class StripeConnectResponse
    {
        public string? AccountId { get; set; }
        public string? OnboardingUrl { get; set; }
        public bool IsSetupComplete { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class StripeConnectStatusResponse
    {
        public bool IsConnected { get; set; }
        public bool CanReceivePayouts { get; set; }
        public string? AccountId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
} 