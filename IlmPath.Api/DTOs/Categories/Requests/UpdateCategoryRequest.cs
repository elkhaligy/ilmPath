using System.ComponentModel.DataAnnotations;

namespace IlmPath.Api.DTOs.Categories.Requests;
public class UpdateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;
}