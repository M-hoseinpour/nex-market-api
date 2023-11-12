using AutoMapper;
using market.Models.DTO.Tag;

namespace market.Models.DTO.Product;

public class GetProductShortResult
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Rating { get; set; }
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
            );
    }
}
