using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class LocalFileStorageService : ILocalFileStorageService
{
    private readonly string _storagePath;

    public LocalFileStorageService(string storagePath)
    {
        _storagePath = storagePath;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        string filePath = Path.Combine(_storagePath, file.FileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return filePath;
    }

    public async Task<Stream> GetFileAsync(string filePath)
    {
        return new FileStream(filePath, FileMode.Open);
    }
}