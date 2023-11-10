using Amazon.S3;
using Amazon.S3.Model;
using market.Data.Repository;
using market.Services.FileService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using market.Models.DTO.File;
using File = market.Data.Domain.File;
using market.Data.StaticData;

namespace market.Services.FileService;

public class FileService
{
    private readonly IRandomService _randomService;
    private readonly S3Config _s3Config;
    private readonly IAmazonS3 _s3Client;
    private readonly IRepository<File> _fileRepository;

    public FileService(
        IRandomService randomService,
        IAmazonS3 s3Client,
        S3Config s3Config,
        IRepository<File> fileRepository
    )
    {
        _randomService = randomService;
        _s3Client = s3Client;
        _s3Config = s3Config;
        _fileRepository = fileRepository;
    }

    public async Task<UploadFileResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        var extension = GetExtension(fileName: file.FileName);

        var mimeType = GetMimeType(extension: extension);

        ValidateMimeType(mimeType: mimeType);

        var subDirectory = GetSubDirectory(mimeType: mimeType);

        var randomKey = _randomService.GetSecureAlphaNumericString(len: 16);

        CheckFileSize(length: file.Length, mimeType: mimeType);

        var s3ObjectKey = $"{randomKey}{extension}";

        var request = new PutObjectRequest()
        {
            BucketName = _s3Config.Bucket,
            Key = subDirectory + "/" + s3ObjectKey,
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request, cancellationToken);

        var fileEntity = new File { S3ObjectKey = s3ObjectKey };

        fileEntity.CategoryId =
            subDirectory == FileCategories.Documents.SubDirectory
                ? FileCategories.Documents.Id
                : FileCategories.Pictures.Id;

        await _fileRepository.AddAsync(entity: fileEntity, cancellationToken: cancellationToken);

        return new UploadFileResult
        {
            FileId = fileEntity.Id.Value,
            FileUrl = GetFileUrlLocal(fileKey: s3ObjectKey, subDirectory: subDirectory)
        };
    }

    public async Task<GetFileUrlResponse> GetFileUrl(
        Guid fileId,
        CancellationToken cancellationToken
    )
    {
        var file = await _fileRepository.Table
            .Where(x => x.Id == fileId)
            .Include(x => x.Category)
            .SingleOrDefaultAsync(cancellationToken);

        if (file is null)
            throw new FileNotFoundException();

        var url = GetFileUrlLocal(
            fileKey: file.S3ObjectKey,
            subDirectory: file.Category.SubDirectory
        );

        return new GetFileUrlResponse { Url = url };
    }

    private string GetFileUrlLocal(string fileKey, string subDirectory)
    {
        var fileAddress = $"{_s3Config.ServiceUrl}/";
        fileAddress += $"{_s3Config.Bucket}/";
        fileAddress += GetFileAddressInS3(fileKey: fileKey, subDirectory: subDirectory);

        return fileAddress;
    }

    private static string GetFileAddressInS3(string fileKey, string subDirectory)
    {
        var fileAddress = "";
        if (!string.IsNullOrEmpty(subDirectory))
            fileAddress += $"{subDirectory}/";
        fileAddress += $"{fileKey}";

        return fileAddress;
    }

    private static string GetSubDirectory(string mimeType)
    {
        if (mimeType.ToLower().StartsWith("image/"))
            return FileCategories.Pictures.SubDirectory;
        if (mimeType.ToLower() is "application/pdf")
            return FileCategories.Documents.SubDirectory;

        throw new BadFileException();
    }

    private static void ValidateMimeType(string mimeType)
    {
        if (mimeType.ToLower().StartsWith("image/"))
            return;
        if (mimeType.ToLower() is "application/pdf")
            return;

        throw new BadFileException();
    }

    private static string GetMimeType(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentNullException(nameof(extension));

        if (extension.StartsWith(".") is false)
            extension = "." + extension;

        var provider = new FileExtensionContentTypeProvider
        {
            Mappings = { [".apk"] = "application/vnd.android.package-archive" }
        };

        if (provider.TryGetContentType(extension, out var contentType) is false)
            throw new BadFileException();

        return contentType;
    }

    private static string GetExtension(string fileName)
    {
        return Path.GetExtension(fileName);
    }

    private static void CheckFileSize(long length, string mimeType)
    {
        const int allowedLength = 10 * 1024 * 1024;

        if (
            (
                mimeType.StartsWith("image/")
                || mimeType == "application/pdf"
                || mimeType == "application/x-zip-compressed"
            )
            && length > allowedLength
        )
        {
            throw new BadFileSizeException();
        }
    }
}