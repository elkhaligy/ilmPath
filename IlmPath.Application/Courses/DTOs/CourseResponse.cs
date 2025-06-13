using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Courses.DTOs
{
    public class CourseResponse
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } 
        public string InstructorId { get; set; }
        public string? InstructorName { get; set; } 
    }
}
