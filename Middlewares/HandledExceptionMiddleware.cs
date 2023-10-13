using System.Text.Json;
using System.Text.Json.Serialization;
using ApiFramework.Models;

namespace ApiFramework.Middlewares;

public class HandledExceptionMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public HandledExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HandledExceptionMiddleware>();
        _next = next;
    }

    [Obsolete]
    public async Task InvokeAsync(HttpContext httpContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        try
        {
            await _next(httpContext);
        }
        catch (HandledException handledException)
        {
            _logger.LogInformation(
                exception: handledException,
                message: "Captured a Handled Exception"
            );
            await HandleExceptionAsync(context: httpContext, exception: handledException);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, HandledException exception)
    {
        var result = new ApiResult(
            statusCode: exception.StatusCode,
            errors: new ApiError[]
            {
                new(type: exception.GetType().Name, message: exception.Message)
            }
        );

        return WriteApiResult(context: context, result: result);
    }

    private Task WriteApiResult(HttpContext context, ApiResult result)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        options.Converters.Add(new JsonStringEnumConverter());

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)result.StatusCode;
        return context.Response.WriteAsync(
            JsonSerializer.Serialize(value: result, options: options)
        );
    }
}
