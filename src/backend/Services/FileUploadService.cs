using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class FileUploadService
    {
        private readonly string _contentRoot;

        public FileUploadService(IWebHostEnvironment host)
        {
            _contentRoot = host.WebRootPath;
        }

        public async Task<string> UploadFormFileAsync(IFormFile file, string dir)
        {
            if (file == null || file.Length == 0)
                return null;

            var dirPath = Path.Combine(_contentRoot, dir);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var relativePath = Path.Combine(dir, fileName);
            var writePath = Path.Combine(dirPath, fileName);
            
            await using (var stream = File.Create(writePath))
            {
                await file.CopyToAsync(stream);
            }

            return relativePath;
        }

        public void DeleteFile(string path)
        {
            var fullPath = Path.Combine(_contentRoot, path);
            File.Delete(fullPath);
        }
    }
}