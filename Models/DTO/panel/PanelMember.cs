using market.Models.Domain;
using AutoMapper;

namespace market.Models.DTO.panel;

public class PanelMember
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public string? UserState { get; set; }
}

public class PanelMemberMap : Profile
{
    public PanelMemberMap()
    {
        CreateMap<Staff, PanelMember>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.User.MobileNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.UserState, opt => opt.MapFrom(src => src.User.UserState.ToString()))
            .ForMember(dest => dest.Role, opt =>
            {
                opt.MapFrom(src => src.User.UserType.ToString());
            });
        CreateMap<Domain.User, PanelMember>()
            .ForMember(dest => dest.Role, opt =>
            {
                opt.MapFrom(src => src.UserType.ToString());
            }); ;
    }
}