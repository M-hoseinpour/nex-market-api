using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using market.Exceptions;
using market.SystemServices;

namespace market.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var tokenSetting = configuration
            .GetSection(nameof(JwtServiceSettings))
            .Get<JwtServiceSettings>();

        services
            .AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(
                options =>
                {
                    var secretKey = Encoding.UTF8.GetBytes(tokenSetting.SignatureKey);

                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidAudience = tokenSetting.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = tokenSetting.Issuer,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = validationParameters;

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception != null)
                            {
                                context.Fail(new UnauthorizedException());
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (context.AuthenticateFailure != null)
                            {
                                return Task.FromException(new UnauthorizedException());
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );
    }
}