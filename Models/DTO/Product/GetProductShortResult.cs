using AutoMapper;
using market.Models.DTO.File;
using market.Models.DTO.Tag;
using market.Models.Enum;

namespace market.Models.DTO.Product;

public class GetProductShortResult
{
    public Guid Uuid { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Rating { get; set; }
    public FileDto? Cover { get; set; }
    public IList<TagResult> Tags { get; set; } = new List<TagResult>();
}

public class GetAllProductsMap : Profile
{
    public GetAllProductsMap()
    {
        CreateMap<Domain.Product, GetProductShortResult>()
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
                                : src.Images.FirstOrDefault(x => x.Type == ProductImageType.Cover)
                                == null
                                    ? null
                                    : src.Images
                                        .FirstOrDefault(x => x.Type == ProductImageType.Cover)
                                        .File
                    )
            );
    }
}
