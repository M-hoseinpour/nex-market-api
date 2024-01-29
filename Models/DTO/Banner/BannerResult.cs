using AutoMapper;
using market.Models.DTO.Brand;
using market.Models.DTO.File;

namespace market.Models.DTO.Banner;

public class BannerResult
{
    public Guid Uuid { get; set; }
    public required string Title { get; set; }
    public string? Detail { get; set; }
    public FileDto File { get; set; } = null!;

}

public class BrandResultMap : Profile
{
    public BrandResultMap()
    {
        CreateMap<Domain.Banner, BannerResult>();
    }
}