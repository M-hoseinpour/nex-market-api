using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ApiFramework.Models;

public class ApiResult<T> : ApiResult
{
    public ApiResult() { }

    public ApiResult(HttpStatusCode statusCode, T result = default) : base(statusCode)
    {
        Result = result;
    }

    public ApiResult(HttpStatusCode statusCode, T result, ICollection<ApiError> errors)
        : base(statusCode: statusCode, errors: errors)
    {
        Result = result;
    }

    public ApiResult(HttpStatusCode statusCode, T result, string error)
        : base(statusCode: statusCode, error: error)
    {
        Result = result;
    }

    public T Result { get; set; }

    #region Implicit Operators
    public static implicit operator ApiResult<T>(OkResult result)
    {
        return new ApiResult<T>((HttpStatusCode)result.StatusCode);
    }

    public static implicit operator ApiResult<T>(OkObjectResult result)
    {
        return new ApiResult<T>(
            statusCode: (HttpStatusCode)result.StatusCode,
            result: (T)result.Value
        );
    }

    public static implicit operator ApiResult<T>(BadRequestResult result)
    {
        return new ApiResult<T>((HttpStatusCode)result.StatusCode);
    }

    public static implicit operator ApiResult<T>(BadRequestObjectResult result)
    {
        if (result.Value is SerializableError errors)
            return new ApiResult<T>(
                statusCode: (HttpStatusCode)result.StatusCode,
                result: default(T),
                errors: errors
                    .Select(q => new ApiError(type: q.Key, message: q.Value.ToString()))
                    .ToList()
            );

        return new ApiResult<T>(
            statusCode: (HttpStatusCode)result.StatusCode,
            result: default(T),
            errors: new ApiError[] { new(result.Value.ToString()) }
        );
    }

    public static implicit operator ApiResult<T>(ContentResult result)
    {
        return new ApiResult<T>(
            statusCode: (HttpStatusCode)result.StatusCode,
            result: JsonSerializer.Deserialize<T>(result.Content)
        );
    }

    public static implicit operator ApiResult<T>(NotFoundResult result)
    {
        return new ApiResult<T>((HttpStatusCode)result.StatusCode);
    }

    public static implicit operator ApiResult<T>(NotFoundObjectResult result)
    {
        return new ApiResult<T>(
            statusCode: (HttpStatusCode)result.StatusCode,
            result: (T)result.Value
        );
    }

    public static implicit operator ApiResult<T>(ObjectResult result)
    {
        return new ApiResult<T>(
            statusCode: (HttpStatusCode)(result.StatusCode ?? 200),
            result: (T)result.Value
        );
    }
    #endregion
}

public class ApiResult
{
    public ApiResult() { }

    public ApiResult(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public ApiResult(HttpStatusCode statusCode, ICollection<ApiError> errors)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public ApiResult(HttpStatusCode statusCode, string error)
    {
        StatusCode = statusCode;
        Errors = new List<ApiError>() { new() };
    }

    public HttpStatusCode? StatusCode { get; set; }
    public ICollection<ApiError> Errors { get; set; }
}
