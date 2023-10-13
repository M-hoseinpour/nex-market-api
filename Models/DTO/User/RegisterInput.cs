namespace market.Models.DTO.User;

public class RegisterInput
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}