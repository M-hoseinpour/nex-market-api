namespace market.Models.DTO.File;

public class S3Config
{
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string ServiceUrl { get; set; } = null!;
    public string Bucket { get; set; } = null!;
}