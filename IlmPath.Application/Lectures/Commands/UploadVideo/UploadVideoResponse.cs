namespace IlmPath.Application.Lectures.Commands.UploadVideo
{
    public record UploadVideoResponse
    {
        public string VideoUrl { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public double DurationInSeconds { get; set; }
    }
} 