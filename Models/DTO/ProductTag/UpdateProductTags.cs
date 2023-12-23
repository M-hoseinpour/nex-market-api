using market.Models.Enum;

namespace market.Models.DTO.ProductTag;

public class UpdateProductTags
{
    public Guid TagUuid { get; set; }
    public UpdateState State { get; set; }
}