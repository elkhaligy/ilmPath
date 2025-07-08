using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IlmPath.Application.Common.Interfaces
{
    public interface IVideoUploadService
    {
        Task<string> UploadVideoAsync(IFormFile videoFile, string lectureId);
        Task<bool> DeleteVideoAsync(string videoUrl);
    }
} 