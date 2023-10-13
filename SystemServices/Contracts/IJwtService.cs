using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace market.SystemServices.Contracts;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, ref long expiresIn);
    JwtSecurityToken Validate(string token, bool validateLifetime = false);
    string GenerateVerificationToken(IEnumerable<Claim> claims);
    JwtSecurityToken ValidateVerification(string token);
}