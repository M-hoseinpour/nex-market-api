using AutoMapper;

namespace market.Models.DTO.Review;

public class ReviewResult : ReviewInput
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class ReviewResultMap : Profile
{
    public ReviewResultMap()
    {
        CreateMap<Domain.Review, ReviewResult>()
            .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.Customer.User.FirstName))
            .ForMember(des => des.LastName, opt => opt.MapFrom(src => src.Customer.User.LastName));
    }
}
