using System.Net;

namespace market.Exceptions;

public class ForbiddenException : HandledException
{
    public ForbiddenException() : base(HttpStatusCode.Forbidden) { }

    public ForbiddenException(string message)
        : base(httpStatusCode: HttpStatusCode.Forbidden, message: message) { }
}
