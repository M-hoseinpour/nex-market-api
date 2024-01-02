using AutoMapper;
using market.Models.DTO.File;
using market.Models.Enum;

namespace market.Models.DTO.Product;

public class GetProductResult : GetProductShortResult
{
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public IList<FileDto>? GalleryImages { get; set; }
}

public class GetProductResponseMap : Profile
{
    public GetProductResponseMap()
    {
        CreateMap<Domain.Product, GetProductResult>()
            .ForMember(
                des => des.Tags,
                opt => opt.MapFrom(src => src.ProductTags.Select(x => x.Tag))
            )
            .ForMember(
                des => des.Cover,
                opt =>
                    opt.MapFrom(
                        src =>
                            src.Images == null
                                ? null
                                : src.Images.FirstOrDefault(x => x.Type == ProductImageType.Cover) == null
                                    ? null
                                    : src.Images.FirstOrDefault(x => x.Type == ProductImageType.Cover).File
                    )
            );
    }
}