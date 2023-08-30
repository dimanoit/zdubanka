using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureBlobOptions _azureBlobOptions;

        public FileService(IOptions<AzureBlobOptions> applicationSettings)
        {
            _azureBlobOptions = applicationSettings.Value;
            _blobServiceClient = new BlobServiceClient(_azureBlobOptions.ConnectionString ??
                                                       throw new ArgumentNullException(nameof(applicationSettings)));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_azureBlobOptions.ContainerName);

            var uniqueBlobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var contentType = GetContentType(file.ContentType);

            var blobClient = containerClient.GetBlobClient(uniqueBlobName);
            await blobClient.UploadAsync(file.OpenReadStream(),
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                });

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_azureBlobOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(filePath);
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            return download.Content;
        }

        private static string GetContentType(string fileContentType)
        {
            return fileContentType switch
            {
                "image/jpeg" => "image/jpeg",
                "image/png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}