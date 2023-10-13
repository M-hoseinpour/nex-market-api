using System.Net;

namespace market.Exceptions
{
    public class UnauthorizedException : HandledException
    {
        public UnauthorizedException() : base(HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}