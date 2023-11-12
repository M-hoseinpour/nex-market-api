using market.Models.DTO.BaseDto;
using market.Models.Enum;

namespace market.Models.DTO.Order;

public class GetOrdersQueryParams : PaginationQueryParams
{
    public OrderStatus? Status { get; set; }
}