using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain;

public class Lecture
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid SectionId { get; set; } // FK to Section
    [ForeignKey("SectionId")]
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