using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class FilesController : ControllerBase
{
    private readonly IAzureBlobStorageService _blobStorageService;

    public FilesController(IAzureBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        string blobName = await _blobStorageService.UploadFileAsync(file);
        return Ok(new { BlobName = blobName });
    }

    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> DownloadFile(string blobName)
    {
        Stream fileStream = await _blobStorageService.DownloadFileAsync(blobName);
        return File(fileStream, "application/octet-stream", blobName, true);
    }
}