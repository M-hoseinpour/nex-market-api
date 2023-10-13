using AutoMapper;
using market.Models.Enum;
namespace market.Models.Domain;
public class ProfileBriefResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? MobileNumber { get; set; }
    public required string Email { get; set; }
    public Role Role { get; set; }
    public UserState UserState { get; set; }
    public string? AvatarLogo { get; set; }
    public string? EmailVerifiedAt { get; set; }
    public string? Setting { get; set; }
}

public class ProfileBriefResponseMap : Profile
{
    public ProfileBriefResponseMap()
    {
        CreateMap<User, ProfileBriefResponse>();
    }
}