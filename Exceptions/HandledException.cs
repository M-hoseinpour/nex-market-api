using System.Net;

public class HandledException : Exception
{
    public readonly HttpStatusCode StatusCode;

    public HandledException(HttpStatusCode httpStatusCode)
    {
        StatusCode = httpStatusCode;
    }

    public HandledException(HttpStatusCode httpStatusCode, string message) : base(message)
    {
        StatusCode = httpStatusCode;
    }
}
