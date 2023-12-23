using AutoMapper;
using market.Models.DTO.File;
using market.Models.Enum;

namespace market.Models.DTO.Product;

public class GetPanelProductResult : GetPanelAllProductsResult
{
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public IList<FileDto>? Images { get; set; }

}

public class GetProductResultByAdminMap : Profile
{
    public GetProductResultByAdminMap()
    {
        CreateMap<Domain.Product, GetPanelProductResult>()
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
                    )
            )
            .ForMember(
                des => des.Images,
                opt =>
                    opt.MapFrom(
                        src =>
                            src.Images == null
                                ? null
                                : src.Images.Where(x => x.Type == ProductImageType.Gallery).ToList()
                    )
            );
    }
}