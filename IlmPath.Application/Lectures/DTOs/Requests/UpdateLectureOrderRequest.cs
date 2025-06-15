using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Lectures.DTOs.Requests
{
    public class UpdateLectureOrderRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Order must be a positive number")]
        public int Order { get; set; }
    }
} 