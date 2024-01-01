using market.Models.DTO.BaseDto;

namespace market.Models.DTO.Product;

public class GetAllProductsQueryParams : PaginationQueryParams
{
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public bool IsDiscount { get; set; } = false;
    public GetAllProductsOrder Order { get; set; } = GetAllProductsOrder.Newest;
}

public enum GetAllProductsOrder
{
    Newest,
    Cheapest,
    Discounted
}