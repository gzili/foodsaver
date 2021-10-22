using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class FileUploadService
    {
        private const string BaseDir = "wwwroot";

        public async Task<string> UploadFormFileAsync(IFormFile file, string dir)
        {
            if (file == null || file.Length == 0)
                return null;
            
            var relativePath = Path.Combine(dir, Path.GetRandomFileName() + Path.GetExtension(file.FileName));
            var writePath = Path.Combine(BaseDir, relativePath);
            
            await using (var stream = File.Create(writePath))
            {
                await file.CopyToAsync(stream);
            }

            return relativePath;
        }
    }
}