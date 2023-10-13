using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace market.Configuration.Swagger;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.DeclaringType
            ?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>();

        if (authAttributes == null || !authAttributes.Any())
            return;

        var securityRequirement = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                },
                new List<string>()
            }
        };
        operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };

        var requiredScopes = authAttributes
            .Select(attr => attr.Policy)
            .Distinct()
            .Where(attr => attr != null);

        if (requiredScopes.Any())
        {
            operation.Responses.Add(
                key: "Authorize",
                value: new OpenApiResponse
                {
                    Description =
                        "Special Permissions: ["
                        + string.Join(separator: ",", values: requiredScopes)
                        + "]"
                }
            );
        }

        operation.Responses.Add(
            key: "401",
            value: new OpenApiResponse { Description = "Unauthorized" }
        );
        operation.Responses.Add(
            key: "403",
            value: new OpenApiResponse { Description = "Forbidden" }
        );
    }
}