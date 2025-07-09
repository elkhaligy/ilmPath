using IlmPath.Application.Lectures.DTOs;
using System.Collections.Generic;

namespace IlmPath.Application.Sections.DTOs
{
    public class SectionWithLecturesResponse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        
        // Calculated fields
        public int DurationMinutes { get; set; }
        public int LecturesCount { get; set; }
        
        // Nested lectures
        public List<LectureResponse> Lectures { get; set; } = new List<LectureResponse>();
    }
} 