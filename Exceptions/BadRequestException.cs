using System.Net;

namespace market.Exceptions
{
    public class BadRequestException : HandledException
    {
        public BadRequestException() : base(HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}