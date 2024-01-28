using AutoMapper;
using market.Models.DTO.Product;

namespace market.Models.DTO.Cart;

public class CartResultDto
{
    public GetProductShortResult Product { get; set; } = null!;
    public int Quantity { get; set; } = 1;
}

public class CartResultDtoMap : Profile
{
    public CartResultDtoMap()
    {
        CreateMap<Domain.CartItem, CartResultDto>().ForMember(des => des.Product, opt => opt.MapFrom(src => src.Product));
    }
}
