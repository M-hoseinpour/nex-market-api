using AutoMapper;

namespace market.Models.DTO.Order;

public class GetOrderFullResult : GetOrderShortResult
{
    public string Address { get; set; }
    public IList<OrderDetailResult> OrderDetails { get; set; }
}

public class GetOrderFullResultMap : Profile
{
    public GetOrderFullResultMap()
    {
        CreateMap<Domain.Order, GetOrderFullResult>()
            .ForMember(des => des.ProductsCount, opt => opt.MapFrom(src => src.OrderDetails.Count))
            .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address.Location));
    }
}