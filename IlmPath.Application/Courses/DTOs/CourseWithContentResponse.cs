using IlmPath.Application.Sections.DTOs;
using System.Collections.Generic;

namespace IlmPath.Application.Courses.DTOs
{
    public class CourseWithContentResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string InstructorId { get; set; } = string.Empty;
        public string? InstructorName { get; set; }
        
        // Learning-specific data
        public int TotalDurationMinutes { get; set; }
        public int TotalLecturesCount { get; set; }
        public int SectionsCount { get; set; }
        
        // Nested content
        public List<SectionWithLecturesResponse> Sections { get; set; } = new List<SectionWithLecturesResponse>();
    }
} 