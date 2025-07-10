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
using IlmPath.Application.Enrollments.Queries.GetAllEnrollments;
using IlmPath.Application.Courses.Queries.GetAllCourses;
using AutoMapper;
using IlmPath.Application.Enrollments.DTOs.Responses;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public AnalyticsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/analytics/instructor/enrollments
        [HttpGet("instructor/enrollments")]
        [ProducesResponseType(typeof(PagedResult<InstructorEnrollmentAnalytics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<InstructorEnrollmentAnalytics>>> GetInstructorEnrollments(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 1000,
            [FromQuery] string? instructorId = null)
        {
            var currentUserId = GetCurrentUserId();
            var targetInstructorId = instructorId ?? currentUserId;

            // For security, only allow instructors to view their own data
            if (instructorId != null && instructorId != currentUserId)
            {
                return Forbid("You can only view your own analytics data");
            }

            // Get all enrollments with course information
            var query = new GetAllEnrollmentsQuery(pageNumber, pageSize, null, targetInstructorId);
            var (enrollments, totalCount) = await _mediator.Send(query);

            var analyticsData = enrollments.Select(e => new InstructorEnrollmentAnalytics
            {
                Id = e.Id,
                UserId = e.UserId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate.ToString("yyyy-MM-dd"),
                PricePaid = e.PricePaid,
                CourseName = e.Course?.Title ?? "Unknown Course",
                UserName = e.User?.UserName ?? "Unknown User"
            }).ToList();

            return Ok(new PagedResult<InstructorEnrollmentAnalytics>(analyticsData, totalCount, pageNumber, pageSize));
        }

        // GET: api/analytics/instructor/revenue-summary
        [HttpGet("instructor/revenue-summary")]
        [ProducesResponseType(typeof(InstructorRevenueSummary), StatusCodes.Status200OK)]
        public async Task<ActionResult<InstructorRevenueSummary>> GetInstructorRevenueSummary(
            [FromQuery] int days = 30)
        {
            var instructorId = GetCurrentUserId();
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            // Get enrollments for the instructor within the date range
            var query = new GetAllEnrollmentsQuery(1, 10000, null, instructorId);
            var (enrollments, _) = await _mediator.Send(query);

            var recentEnrollments = enrollments.Where(e => e.EnrollmentDate >= cutoffDate).ToList();
            var allTimeEnrollments = enrollments.ToList();

            var summary = new InstructorRevenueSummary
            {
                TotalRevenue = allTimeEnrollments.Sum(e => e.PricePaid),
                RevenueInPeriod = recentEnrollments.Sum(e => e.PricePaid),
                TotalStudents = allTimeEnrollments.GroupBy(e => e.UserId).Count(),
                StudentsInPeriod = recentEnrollments.GroupBy(e => e.UserId).Count(),
                TotalEnrollments = allTimeEnrollments.Count,
                EnrollmentsInPeriod = recentEnrollments.Count,
                Period = $"Last {days} days"
            };

            return Ok(summary);
        }

        // GET: api/analytics/instructor/course-performance
        [HttpGet("instructor/course-performance")]
        [ProducesResponseType(typeof(List<CoursePerformanceAnalytics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CoursePerformanceAnalytics>>> GetCoursePerformance()
        {
            var instructorId = GetCurrentUserId();

            // Get instructor's courses
            var coursesQuery = new GetAllCoursesQuery(1, 1000);
            var coursesResult = await _mediator.Send(coursesQuery);
            var instructorCourses = coursesResult.Items.Where(c => c.InstructorId == instructorId).ToList();

            // Get all enrollments for the instructor
            var enrollmentsQuery = new GetAllEnrollmentsQuery(1, 10000, null, instructorId);
            var (enrollments, _) = await _mediator.Send(enrollmentsQuery);

            var coursePerformance = instructorCourses.Select(course =>
            {
                var courseEnrollments = enrollments.Where(e => e.CourseId == course.Id).ToList();
                
                return new CoursePerformanceAnalytics
                {
                    CourseId = course.Id,
                    CourseTitle = course.Title,
                    TotalEnrollments = courseEnrollments.Count,
                    TotalRevenue = courseEnrollments.Sum(e => e.PricePaid),
                    AverageRating = 4.5m, // Placeholder - would need rating system
                    CompletionRate = 75m, // Placeholder - would need progress tracking
                    IsPublished = course.IsPublished,
                    Price = course.Price,
                    ThumbnailImageUrl = course.ThumbnailImageUrl
                };
            }).ToList();

            return Ok(coursePerformance);
        }

        // GET: api/analytics/instructor/monthly-trends
        [HttpGet("instructor/monthly-trends")]
        [ProducesResponseType(typeof(List<MonthlyTrend>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MonthlyTrend>>> GetMonthlyTrends(
            [FromQuery] int months = 6)
        {
            var instructorId = GetCurrentUserId();

            // Get all enrollments for the instructor
            var query = new GetAllEnrollmentsQuery(1, 10000, null, instructorId);
            var (enrollments, _) = await _mediator.Send(query);

            var monthlyData = enrollments
                .GroupBy(e => new { e.EnrollmentDate.Year, e.EnrollmentDate.Month })
                .Select(group => new MonthlyTrend
                {
                    Month = $"{group.Key.Year}-{group.Key.Month:D2}",
                    Revenue = group.Sum(e => e.PricePaid),
                    Enrollments = group.Count(),
                    UniqueStudents = group.GroupBy(e => e.UserId).Count()
                })
                .OrderByDescending(m => m.Month)
                .Take(months)
                .OrderBy(m => m.Month)
                .ToList();

            return Ok(monthlyData);
        }
    }

    // DTOs for Analytics
    public class InstructorEnrollmentAnalytics
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string EnrollmentDate { get; set; } = string.Empty;
        public decimal PricePaid { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class InstructorRevenueSummary
    {
        public decimal TotalRevenue { get; set; }
        public decimal RevenueInPeriod { get; set; }
        public int TotalStudents { get; set; }
        public int StudentsInPeriod { get; set; }
        public int TotalEnrollments { get; set; }
        public int EnrollmentsInPeriod { get; set; }
        public string Period { get; set; } = string.Empty;
    }

    public class CoursePerformanceAnalytics
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int TotalEnrollments { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageRating { get; set; }
        public decimal CompletionRate { get; set; }
        public bool IsPublished { get; set; }
        public decimal Price { get; set; }
        public string? ThumbnailImageUrl { get; set; }
    }

    public class MonthlyTrend
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int Enrollments { get; set; }
        public int UniqueStudents { get; set; }
    }
} 