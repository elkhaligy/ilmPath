namespace IlmPath.Application.Common.Models
{
    public class VideoUploadResult
    {
        public string VideoUrl { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public double DurationInSeconds { get; set; }
    }
} 