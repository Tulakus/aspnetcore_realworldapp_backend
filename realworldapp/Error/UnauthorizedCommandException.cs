using System.Net;
using realworldapp.Infrastructure;

namespace realworldapp.Error
{
    public class UnauthorizedCommandException : HttpCommandException
    {
        public UnauthorizedCommandException() : base(HttpStatusCode.Unauthorized, new
        {
            User = ErrorMessages.YouMustLoginFirst
        })

        {
        }
    }
}
