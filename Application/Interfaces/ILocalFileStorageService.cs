using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface ILocalFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<Stream> GetFileAsync(string filePath);
}