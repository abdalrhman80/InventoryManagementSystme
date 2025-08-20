using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Infrastructure.Services
{
    internal class FileService(IWebHostEnvironment _webHostEnvironment) : IFileService
    {
        private readonly string _rootPath = _webHostEnvironment.WebRootPath;

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            var uploadFolder = Path.Combine(_rootPath, folderPath);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, fileName);

            await FileStreaming(file, filePath);

            return Path.Combine(folderPath, fileName).Replace('\\', '/');
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_rootPath, filePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private static async Task FileStreaming(IFormFile file, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
    }
}
