using AutoMapper;
using market.Models.DTO.File;

namespace market.Models.DTO.Brand;

public class BrandResult
{
    public required string Name { get; set; }
    public Guid Uuid { get; set; }
    public FileDto? LogoFile { get; set; }
}

public class BrandResultMap : Profile
{
    public BrandResultMap()
    {
        CreateMap<Domain.Brand, BrandResult>();
    }
}