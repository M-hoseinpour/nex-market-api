using AutoMapper;
using market.Models.Domain;
using market.Models.Enum;

namespace market.Models.DTO.Product;

public class AddProductInput
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
    public Guid BrandGuid { get; set; }
    public Guid CategoryGuid { get; set; }
    public IList<Guid> TagIds { get; set; } = new List<Guid>();
    
    public IList<ProductImageInput>? ProductImages { get; set; }
}

public class AddProductInputMap : Profile
{
    public AddProductInputMap()
    {
        CreateMap<AddProductInput, Domain.Product>();
    }
}