using AutoMapper;
using market.Models.DTO.File;

namespace market.Models.DTO.User;
public class ProfileBriefResponse
{
    public Guid Uuid { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public string? UserState { get; set; }
    public FileDto? AvatarFile { get; set; }    
    public string? EmailVerifiedAt { get; set; }
    public string? Setting { get; set; }
    public Guid? PanelGuid { get; set; }
}

public class ProfileBriefResponseMap : Profile
{
    public ProfileBriefResponseMap()
    {
        CreateMap<Domain.User, ProfileBriefResponse>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserType.ToString()))
        .ForMember(dest => dest.UserState, opt => opt.MapFrom(src => src.UserState.ToString())); // Maps the enum string representation

    }
}