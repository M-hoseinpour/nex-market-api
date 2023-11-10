namespace market.Models.DTO.File;

public class UploadFileResult
{
    public Guid FileId { get; set; }
    public string FileUrl { get; set; } = null!;
}