using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAzureBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<Stream> DownloadFileAsync(string blobName);
}