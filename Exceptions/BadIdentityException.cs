using market.Exceptions;

namespace ApiFramework.Exceptions;

public class BadIdentityException : BadRequestException
{
    public BadIdentityException() { }

    public BadIdentityException(string message) : base(message) { }
}
