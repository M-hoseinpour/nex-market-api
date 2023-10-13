using System.Net;

namespace market.Exceptions
{
    public class NotFoundException : HandledException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}