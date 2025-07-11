using IlmPath.Application.Common.Pagination;
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
using IlmPath.Application.Payouts.Commands.GenerateInstructorPayout;
using IlmPath.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInstructorPayoutRepository _payoutRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStripeConnectService _stripeConnectService;

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public AdminController(IMediator mediator, IInstructorPayoutRepository payoutRepository, UserManager<ApplicationUser> userManager, IStripeConnectService stripeConnectService)
        {
            _mediator = mediator;
            _payoutRepository = payoutRepository;
            _userManager = userManager;
            _stripeConnectService = stripeConnectService;
        }

        // GET: api/admin/withdrawal-requests
        [HttpGet("withdrawal-requests")]
        [ProducesResponseType(typeof(PagedResult<WithdrawalRequestSummary>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<WithdrawalRequestSummary>>> GetWithdrawalRequests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null)
        {
            var (payouts, totalCount) = await _payoutRepository.GetAllPayoutsAsync(pageNumber, pageSize);

            // Filter by status if provided
            if (!string.IsNullOrEmpty(status))
            {
                payouts = payouts.Where(p => p.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                totalCount = payouts.Count();
            }

            var withdrawalRequests = payouts.Select(p => new WithdrawalRequestSummary
            {
                Id = p.Id,
                InstructorId = p.InstructorId,
                InstructorName = p.Instructor != null ? $"{p.Instructor.FirstName} {p.Instructor.LastName}" : "Unknown",
                InstructorEmail = p.Instructor?.Email ?? "Unknown",
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

            return Ok(new PagedResult<WithdrawalRequestSummary>(withdrawalRequests, totalCount, pageNumber, pageSize));
        }

        // GET: api/admin/withdrawal-requests/{id}
        [HttpGet("withdrawal-requests/{id}")]
        [ProducesResponseType(typeof(WithdrawalRequestDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WithdrawalRequestDetails>> GetWithdrawalRequestById(int id)
        {
            var payout = await _payoutRepository.GetPayoutByIdAsync(id);
            if (payout == null)
            {
                return NotFound($"Withdrawal request with ID {id} not found");
            }

            var details = new WithdrawalRequestDetails
            {
                Id = payout.Id,
                InstructorId = payout.InstructorId,
                InstructorName = payout.Instructor != null ? $"{payout.Instructor.FirstName} {payout.Instructor.LastName}" : "Unknown",
                InstructorEmail = payout.Instructor?.Email ?? "Unknown",
                GrossAmount = payout.GrossAmount,
                CommissionRate = payout.CommissionRate,
                CommissionAmount = payout.CommissionAmount,
                NetAmount = payout.NetAmount,
                Status = payout.Status,
                PaymentMethod = payout.PaymentMethod,
                ExternalTransactionId = payout.ExternalTransactionId,
                RequestDate = payout.PayoutDate,
                ProcessedDate = payout.ProcessedDate,
                Notes = payout.Notes,
                EnrollmentsIncluded = payout.PayoutEnrollments.Select(pe => new EnrollmentInPayout
                {
                    EnrollmentId = pe.EnrollmentId,
                    StudentName = pe.Enrollment?.User != null ? $"{pe.Enrollment.User.FirstName} {pe.Enrollment.User.LastName}" : "Unknown",
                    CourseName = pe.Enrollment?.Course?.Title ?? "Unknown",
                    AmountPaid = pe.Enrollment?.PricePaid ?? 0,
                    InstructorShare = pe.AmountPaidToInstructor,
                    EnrollmentDate = pe.Enrollment?.EnrollmentDate ?? DateTime.MinValue
                }).ToList()
            };

            return Ok(details);
        }

        // PUT: api/admin/withdrawal-requests/{id}/approve
        [HttpPut("withdrawal-requests/{id}/approve")]
        [ProducesResponseType(typeof(WithdrawalRequestSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WithdrawalRequestSummary>> ApproveWithdrawalRequest(
            int id, 
            [FromBody] ApproveWithdrawalRequest request)
        {
            var payout = await _payoutRepository.GetPayoutByIdAsync(id);
            if (payout == null)
            {
                return NotFound($"Withdrawal request with ID {id} not found");
            }

            if (payout.Status != "Pending")
            {
                return BadRequest($"Cannot approve request with status '{payout.Status}'. Only pending requests can be approved.");
            }

            // Get instructor's Stripe Connect account
            var instructor = await _userManager.FindByIdAsync(payout.InstructorId);
            if (instructor == null)
            {
                return BadRequest("Instructor not found");
            }

            if (string.IsNullOrEmpty(instructor.StripeConnectAccountId))
            {
                return BadRequest("Instructor has not set up their Stripe Connect account. Cannot process payout.");
            }

            // Validate Stripe account can receive payouts
            var canReceivePayouts = await _stripeConnectService.CanReceivePayoutsAsync(instructor.StripeConnectAccountId);
            if (!canReceivePayouts)
            {
                return BadRequest("Instructor's Stripe account is not ready to receive payouts. Please ask them to complete their account setup.");
            }

            try
            {
                // Send payout via Stripe Connect automatically
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
                    payout.Notes = (payout.Notes ?? "") + $"\nAdmin approved and automatically sent payout to Stripe account {instructor.StripeConnectAccountId}";
                    
                    if (!string.IsNullOrEmpty(request.AdminNotes))
                    {
                        payout.Notes += $"\nAdmin notes: {request.AdminNotes}";
                    }
                }
                else
                {
                    // If payout failed, just approve but don't complete
                    payout.Status = "Approved";
                    payout.ProcessedDate = DateTime.UtcNow;
                    payout.Notes = (payout.Notes ?? "") + $"\nAdmin approved but payout failed: {payoutResult.ErrorMessage}";
                    
                    if (!string.IsNullOrEmpty(request.AdminNotes))
                    {
                        payout.Notes += $"\nAdmin notes: {request.AdminNotes}";
                    }
                    
                    await _payoutRepository.UpdatePayoutAsync(payout);
                    return BadRequest($"Approval successful but payout failed: {payoutResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // If there's an error with Stripe, just approve but don't complete
                payout.Status = "Approved";
                payout.ProcessedDate = DateTime.UtcNow;
                payout.Notes = (payout.Notes ?? "") + $"\nAdmin approved but payout failed with error: {ex.Message}";
                
                if (!string.IsNullOrEmpty(request.AdminNotes))
                {
                    payout.Notes += $"\nAdmin notes: {request.AdminNotes}";
                }
                
                await _payoutRepository.UpdatePayoutAsync(payout);
                return BadRequest($"Approval successful but payout failed: {ex.Message}");
            }

            await _payoutRepository.UpdatePayoutAsync(payout);

            var summary = new WithdrawalRequestSummary
            {
                Id = payout.Id,
                InstructorId = payout.InstructorId,
                InstructorName = payout.Instructor != null ? $"{payout.Instructor.FirstName} {payout.Instructor.LastName}" : "Unknown",
                InstructorEmail = payout.Instructor?.Email ?? "Unknown",
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

        // PUT: api/admin/withdrawal-requests/{id}/reject
        [HttpPut("withdrawal-requests/{id}/reject")]
        [ProducesResponseType(typeof(WithdrawalRequestSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WithdrawalRequestSummary>> RejectWithdrawalRequest(
            int id, 
            [FromBody] RejectWithdrawalRequest request)
        {
            var payout = await _payoutRepository.GetPayoutByIdAsync(id);
            if (payout == null)
            {
                return NotFound($"Withdrawal request with ID {id} not found");
            }

            if (payout.Status != "Pending")
            {
                return BadRequest($"Cannot reject request with status '{payout.Status}'. Only pending requests can be rejected.");
            }

            // Update payout status to rejected
            payout.Status = "Rejected";
            payout.ProcessedDate = DateTime.UtcNow;
            payout.Notes = (payout.Notes ?? "") + $"\nAdmin rejection: {request.Reason}";

            await _payoutRepository.UpdatePayoutAsync(payout);

            var summary = new WithdrawalRequestSummary
            {
                Id = payout.Id,
                InstructorId = payout.InstructorId,
                InstructorName = payout.Instructor != null ? $"{payout.Instructor.FirstName} {payout.Instructor.LastName}" : "Unknown",
                InstructorEmail = payout.Instructor?.Email ?? "Unknown",
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

        // GET: api/admin/dashboard-stats
        [HttpGet("dashboard-stats")]
        [ProducesResponseType(typeof(AdminDashboardStats), StatusCodes.Status200OK)]
        public async Task<ActionResult<AdminDashboardStats>> GetDashboardStats()
        {
            var (allPayouts, _) = await _payoutRepository.GetAllPayoutsAsync(1, 10000);

            var stats = new AdminDashboardStats
            {
                PendingRequestsCount = allPayouts.Count(p => p.Status == "Pending"),
                ApprovedRequestsCount = allPayouts.Count(p => p.Status == "Approved"),
                RejectedRequestsCount = allPayouts.Count(p => p.Status == "Rejected"),
                CompletedRequestsCount = allPayouts.Count(p => p.Status == "Completed"),
                TotalPendingAmount = allPayouts.Where(p => p.Status == "Pending").Sum(p => p.NetAmount),
                TotalApprovedAmount = allPayouts.Where(p => p.Status == "Approved").Sum(p => p.NetAmount),
                TotalCompletedAmount = allPayouts.Where(p => p.Status == "Completed").Sum(p => p.NetAmount),
                LastUpdated = DateTime.UtcNow
            };

            return Ok(stats);
        }
    }

    // DTOs for Admin Controller
    public class WithdrawalRequestSummary
    {
        public int Id { get; set; }
        public string InstructorId { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string InstructorEmail { get; set; } = string.Empty;
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

    public class WithdrawalRequestDetails
    {
        public int Id { get; set; }
        public string InstructorId { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string InstructorEmail { get; set; } = string.Empty;
        public decimal GrossAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? ExternalTransactionId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? Notes { get; set; }
        public List<EnrollmentInPayout> EnrollmentsIncluded { get; set; } = new List<EnrollmentInPayout>();
    }

    public class EnrollmentInPayout
    {
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal InstructorShare { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class ApproveWithdrawalRequest
    {
        public string? AdminNotes { get; set; }
    }

    public class RejectWithdrawalRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

    public class AdminDashboardStats
    {
        public int PendingRequestsCount { get; set; }
        public int ApprovedRequestsCount { get; set; }
        public int RejectedRequestsCount { get; set; }
        public int CompletedRequestsCount { get; set; }
        public decimal TotalPendingAmount { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal TotalCompletedAmount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 