using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Sections.DTOs.Requests;

public class CreateSectionRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public int Order { get; set; }
} 