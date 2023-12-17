using AutoMapper;

namespace market.Models.DTO.Cart;

public class CartDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class GetOrderShortResultMap : Profile
{
    public GetOrderShortResultMap()
    {
        CreateMap<Domain.CartItem, CartDto>();
    }
}
