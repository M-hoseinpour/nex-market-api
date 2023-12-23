using market.Models.DTO.ProductTag;
using market.Models.Enum;

namespace market.Models.DTO.Product;

public class UpdateProductInput
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
    public Guid BrandGuid { get; set; }
    public Guid CategoryGuid { get; set; }
    public IList<UpdateProductTags> TagIds { get; set; } = new List<UpdateProductTags>();
}