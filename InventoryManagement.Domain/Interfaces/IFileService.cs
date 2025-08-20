using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        void DeleteFile(string filePath);
    }
}
