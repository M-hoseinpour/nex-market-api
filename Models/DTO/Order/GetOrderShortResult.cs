using AutoMapper;
using market.Models.Enum;

namespace market.Models.DTO.Order;

public class GetOrderShortResult
{
    public Guid Uuid { get; set; }
    public OrderStatus Status { get; set; }
    public int ProductsCount { get; set; }
}

public class GetOrderShortResultMap : Profile
{
    public GetOrderShortResultMap()
    {
        CreateMap<Domain.Order, GetOrderShortResult>()
            .ForMember(des => des.ProductsCount, opt => opt.MapFrom(src => src.OrderDetails.Count));
    }
}
