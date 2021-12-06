using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public interface IFileService
    {
        Task<string> UploadFormFileAsync(IFormFile file, string dir);
        void DeleteFile(string path);
    }
}