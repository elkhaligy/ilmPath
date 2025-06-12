using System.ComponentModel.DataAnnotations;

namespace IlmPath.Application.Categories.DTOs.Requests;
public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;
}