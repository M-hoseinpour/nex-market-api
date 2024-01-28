using market.Models.DTO.BaseDto;

namespace market.Models.DTO.Product;

public class GetAllProductsQueryParams : PaginationQueryParams
{
    public string? Title { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public bool IsDiscount { get; set; } = false;
    public GetAllProductsOrder Order { get; set; } = GetAllProductsOrder.Newest;
}

public enum GetAllProductsOrder
{
    Newest,
    Cheapest,
    Discounted
}