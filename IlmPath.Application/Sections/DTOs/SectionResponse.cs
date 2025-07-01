namespace IlmPath.Application.Sections.DTOs;

public class SectionResponse
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
} 