using market.Extensions;
using market.SystemServices.Contracts;

namespace market.SystemServices;

public class PasswordService : IPasswordService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        password.NotNull(nameof(password));
        passwordHash.NotNull(nameof(password));
        return BCrypt.Net.BCrypt.Verify(text: password, hash: passwordHash);
    }
}