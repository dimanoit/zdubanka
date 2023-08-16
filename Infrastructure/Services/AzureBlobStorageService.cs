using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        string connectionString = configuration["AzureBlobStorageSettings:ConnectionString"];
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = "uploads"; // Name of the container in Azure Blob Storage
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        // Generate a unique blob name
        string uniqueBlobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Set the correct Content-Type based on the file type
        string contentType = "application/octet-stream"; // Default content type
        if (file.ContentType == "image/jpeg")
        {
            contentType = "image/jpeg";
        }
        else if (file.ContentType == "image/png")
        {
            contentType = "image/png";
        }
        // Add more conditions for other file types if needed

        // Create a blob client and upload the file with the specified Content-Type
        BlobClient blobClient = containerClient.GetBlobClient(uniqueBlobName);
        await blobClient.UploadAsync(file.OpenReadStream(), new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } });

        return blobClient.Name;
    }

    public async Task<Stream> DownloadFileAsync(string blobName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        return download.Content;
    }
}