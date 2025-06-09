using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class Lecture
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SectionId { get; set; } // FK to Section
    public virtual Section? Section { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string VideoUrl { get; set; } = string.Empty;
    public int? DurationInMinutes { get; set; }
    public int Order { get; set; }
    public bool IsPreviewAllowed { get; set; } = false;
}