using market.Models.Enum;

namespace market.Models.DTO.Product;

public class ProductImageInput
{
    public ProductImageType Type { get; set; }
    public Guid FileId { get; set; }
}