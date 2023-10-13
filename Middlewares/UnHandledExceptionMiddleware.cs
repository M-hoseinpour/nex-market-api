using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiFramework.Models;


namespace ApiFramework.Middlewares;

public class UnHandledExceptionMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public UnHandledExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<UnHandledExceptionMiddleware>();
    }

    [Obsolete]
    public async Task InvokeAsync(HttpContext httpContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception: exception, message: "Captured a Error");
            if (env.IsStaging() || env.IsDevelopment())
                await HandleExceptionAsync(context: httpContext, exception: exception);
            else
                await HandleExceptionAsync(context: httpContext, exception: null);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var result = new ApiResult(
            statusCode: HttpStatusCode.InternalServerError,
            errors: new List<ApiError>()
        );

        if (exception == null)
            return WriteApiResult(context: context, result: result);

        for (var e = exception; e != null; e = e.InnerException)
            result.Errors.Add(
                new ApiError
                {
                    Type = e.GetType().Name,
                    Message = e.Message,
                    StackTrace = e.StackTrace
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
