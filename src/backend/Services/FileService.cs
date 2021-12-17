using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace backend.Services
{
    public class FileService : IFileService
    {
        private readonly string _contentRoot;

        public FileService(IWebHostEnvironment host)
        {
            _contentRoot = host.WebRootPath;
        }

        public async Task<string> UploadFormFileAsync(IFormFile file, string dir)
        {
            if (file == null || file.Length == 0)
            {
                Log.Error("File is null or length is 0");
                return null;
            }

            var dirPath = Path.Combine(_contentRoot, dir);
            if (!Directory.Exists(dirPath))
            {
                Log.Information("Directory \"{dir.Path}\" was created.", dirPath);
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
            Log.Information("File \"{full.Path}\" deleted", fullPath);
        }
    }
}