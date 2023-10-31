using market.Models.Enum;

namespace market.Models.DTO.ProductTag;

public class UpdateProductTags
{
    public int TagId { get; set; }
    public UpdateState State { get; set; }
}