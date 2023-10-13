using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using market.SystemServices.Contracts;

namespace market.SystemServices;

public class JwtService : IJwtService
{
    private readonly JwtServiceSettings _settings;
    private readonly SigningCredentials _signingCredentials;

    private readonly SigningCredentials _verificationSigningCredentials;
    // private readonly EncryptingCredentials _encryptingCredentials;

    public JwtService(JwtServiceSettings settings)
    {
        _settings = settings;
        var signatureKey = Encoding.UTF8.GetBytes(_settings.SignatureKey);
        // var encryptionKey = Encoding.UTF8.GetBytes(_settings.EncryptionKey);
        var verificationSignatureKey = Encoding.UTF8.GetBytes(_settings.VerifySignatureKey);
        _signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(signatureKey),
            algorithm: SecurityAlgorithms.HmacSha256Signature
        );
        // _encryptingCredentials = new EncryptingCredentials(
        //     key: new SymmetricSecurityKey(encryptionKey),
        //     alg: SecurityAlgorithms.Aes256KW,
        //     enc: SecurityAlgorithms.Aes256CbcHmacSha512
        // );
        _verificationSigningCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(verificationSignatureKey),
            algorithm: SecurityAlgorithms.HmacSha256Signature
        );
    }


    public string GenerateToken(IEnumerable<Claim> claims, ref long expiresIn)
    {
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresAfter);
        expiresIn = ((DateTimeOffset)expires).ToUnixTimeMilliseconds();

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = expires,
            SigningCredentials = _signingCredentials,
            // EncryptingCredentials = _encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public JwtSecurityToken Validate(string token, bool validateLifetime = false)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(
            token: token,
            validationParameters: new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingCredentials.Key,
                // TokenDecryptionKey = _encryptingCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidateLifetime = validateLifetime,
                ValidAudience = _settings.Audience,
                ClockSkew = TimeSpan.Zero
            },
            validatedToken: out var validatedToken
        );

        return (JwtSecurityToken)validatedToken;
    }

    public string GenerateVerificationToken(IEnumerable<Claim> claims)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(_settings.VerifyExpiresAfter),
            SigningCredentials = _verificationSigningCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public JwtSecurityToken ValidateVerification(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(
            token: token,
            validationParameters: new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _verificationSigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            },
            validatedToken: out var validatedToken
        );

        return (JwtSecurityToken)validatedToken;
    }
}

public class JwtServiceSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }

    public string SignatureKey { get; set; }

    // public string EncryptionKey { get; set; }
    public int ExpiresAfter { get; set; }
    public string VerifySignatureKey { get; set; }
    public int VerifyExpiresAfter { get; set; }
    public string RefreshTokenSignatureKey { get; set; }
    public int RefreshTokenExpiresAfter { get; set; }
}