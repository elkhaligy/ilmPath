using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Lectures.DTOs.Requests
{
    public class CreateLectureRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int? DurationInMinutes { get; set; }
        public int Order { get; set; }
        public bool IsPreviewAllowed { get; set; } = false;
    }
} 