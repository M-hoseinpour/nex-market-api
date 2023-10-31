using AutoMapper;
using market.Models.DTO.Tag;

namespace market.Models.DTO.Product;

public class GetAllProductsResponse
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Rating { get; set; }
    public IList<TagResponse> Tags { get; set; } = new List<TagResponse>();
}

public class GetAllProductsMap : Profile
{
    public GetAllProductsMap()
    {
        CreateMap<Domain.Product, GetAllProductsResponse>()
            .ForMember(
                des => des.Tags,
                opt => opt.MapFrom(src => src.ProductTags.Select(x => x.Tag))
            );
    }
}
