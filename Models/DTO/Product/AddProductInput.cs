using AutoMapper;
using market.Models.DTO.ProductTag;

namespace market.Models.DTO.Product;

public class AddProductInput
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public IList<UpdateProductTags> TagIds { get; set; } = new List<UpdateProductTags>();
}

public class AddProductInputMap : Profile
{
    public AddProductInputMap()
    {
        CreateMap<AddProductInput, Domain.Product>();
    }
}