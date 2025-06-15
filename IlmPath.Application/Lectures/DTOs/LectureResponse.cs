using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Lectures.DTOs
{
    public class LectureResponse
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int? DurationInMinutes { get; set; }
        public int Order { get; set; }
        public bool IsPreviewAllowed { get; set; }
    }
} 