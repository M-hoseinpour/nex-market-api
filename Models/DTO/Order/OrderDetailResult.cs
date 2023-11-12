using AutoMapper;
using market.Models.Domain;
using market.Models.DTO.Product;

namespace market.Models.DTO.Order;

public class OrderDetailResult
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public GetProductResult Product { get; set; }
}

public class OrderDetailResultMap : Profile
{
    public OrderDetailResultMap()
    {
        CreateMap<OrderDetail, OrderDetailResult>();
    }
}