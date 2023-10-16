using market.Models.Enum;

namespace market.Models.DTO.User;

public class RegisterInput
{
    public required UserType UserType { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}