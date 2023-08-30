using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class LocalFileStorageService : IFileService
{
    private readonly string _storagePath;

    public LocalFileStorageService(string storagePath)
    {
        _storagePath = storagePath;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        string filePath = Path.Combine(_storagePath, file.FileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return filePath;
    }

    public Task<Stream> DownloadFileAsync(string filePath)
    {
        Stream stream = new FileStream(filePath, FileMode.Open);
        return Task.FromResult(stream);
    }
}