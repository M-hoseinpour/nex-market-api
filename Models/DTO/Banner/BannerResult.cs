using market.Models.DTO.File;

namespace market.Models.DTO.Banner;

public class BannerResult
{
    public Guid Uuid { get; set; }
    public required string Title { get; set; }
    public FileDto File { get; set; } = null!;

}