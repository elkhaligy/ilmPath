using IlmPath.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace IlmPath.Api.Controllers
{
    [Route("api/admin/reports")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Make sure your JWT token has "Admin" role claim
    public class AdminReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminReportsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Helper to get the current user's ID from JWT
        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // GET: /api/admin/reports/revenue
        [HttpGet("revenue")]
        [Authorize(Roles = "Admin")] // Explicitly add authorization attribute to each endpoint
        public async Task<IActionResult> GetRevenue([FromQuery] string from, [FromQuery] string to)
        {
            // Check if user is authenticated and has Admin role
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            if (!User.IsInRole("Admin"))
                return StatusCode(403, new { message = "User is not authorized to access this endpoint." });

            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
                return BadRequest(new { message = "Invalid date format." });
            var payments = await _dbContext.Payments
                .Where(p => p.Status == "Completed" && p.PaymentDate >= fromDate && p.PaymentDate <= toDate)
                .ToListAsync();
            var totalRevenue = payments.Sum(p => p.Amount);
            return Ok(new { totalRevenue, paymentCount = payments.Count });
        }

        // GET: /api/admin/reports/user-growth
        [HttpGet("user-growth")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserGrowth([FromQuery] string from, [FromQuery] string to)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            if (!User.IsInRole("Admin"))
                return StatusCode(403, new { message = "User is not authorized to access this endpoint." });

            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
                return BadRequest(new { message = "Invalid date format." });
            var users = await _dbContext.Users
                .Where(u => u.CreatedAt >= fromDate && u.CreatedAt <= toDate)
                .ToListAsync();
            return Ok(new { newUsers = users.Count });
        }

        // GET: /api/admin/reports/top-courses
        [HttpGet("top-courses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopCourses([FromQuery] string from, [FromQuery] string to, [FromQuery] int limit = 10)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            if (!User.IsInRole("Admin"))
                return StatusCode(403, new { message = "User is not authorized to access this endpoint." });

            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
                return BadRequest(new { message = "Invalid date format." });
            var topCourses = await _dbContext.Enrollments
                .Where(e => e.EnrollmentDate >= fromDate && e.EnrollmentDate <= toDate)
                .GroupBy(e => e.CourseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    EnrollmentCount = g.Count()
                })
                .OrderByDescending(x => x.EnrollmentCount)
                .Take(limit)
                .ToListAsync();
            return Ok(topCourses);
        }

        // GET: /api/admin/reports/instructor-earnings
        [HttpGet("instructor-earnings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInstructorEarnings([FromQuery] string from, [FromQuery] string to)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            if (!User.IsInRole("Admin"))
                return StatusCode(403, new { message = "User is not authorized to access this endpoint." });

            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
                return BadRequest(new { message = "Invalid date format." });
            var earnings = await _dbContext.InstructorPayouts
                .Where(p => p.PayoutDate >= fromDate && p.PayoutDate <= toDate)
                .GroupBy(p => p.InstructorId)
                .Select(g => new
                {
                    InstructorId = g.Key,
                    TotalEarnings = g.Sum(x => x.NetAmount)
                })
                .OrderByDescending(x => x.TotalEarnings)
                .ToListAsync();
            return Ok(earnings);
        }

        // GET: /api/admin/reports/withdrawals
        [HttpGet("withdrawals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithdrawals([FromQuery] string from, [FromQuery] string to)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { message = "User is not authenticated." });

            if (!User.IsInRole("Admin"))
                return StatusCode(403, new { message = "User is not authorized to access this endpoint." });

            if (!DateTime.TryParse(from, out var fromDate) || !DateTime.TryParse(to, out var toDate))
                return BadRequest(new { message = "Invalid date format." });
            var withdrawals = await _dbContext.InstructorPayouts
                .Where(p => p.PayoutDate >= fromDate && p.PayoutDate <= toDate)
                .ToListAsync();
            return Ok(withdrawals);
        }
    }
}