using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Lectures.DTOs.Requests
{
    public class ToggleLecturePreviewRequest
    {
        [Required]
        public bool IsPreviewAllowed { get; set; }
    }
} 