using market.Models.DTO.File;
using market.Services.FileService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileService _fileService;

    public FilesController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload-file")]
    public async Task<UploadFileResult> UploadFile(
        IFormFile file,
        CancellationToken cancellationToken
    )
    {
        return await _fileService.Upload(
            file: file,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet("file-url")]
    public async Task<GetFileUrlResponse> GetFileUrl(
        [FromQuery] Guid fileId,
        CancellationToken cancellationToken
    )
    {
        return await _fileService.GetFileUrl(
            fileId: fileId,
            cancellationToken: cancellationToken
        );
    }
}