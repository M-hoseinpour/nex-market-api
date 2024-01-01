using market.Models.DTO.File;

namespace market.Models.DTO.User;

public class ProfileInput
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public Guid? AvatarFileId { get; set; }
}