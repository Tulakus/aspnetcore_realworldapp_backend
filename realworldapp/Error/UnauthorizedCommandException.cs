using System.Net;

namespace realworldapp.Error
{
    public class UnauthorizedCommandException : HttpCommandException
    {
        public UnauthorizedCommandException(): base(HttpStatusCode.Unauthorized, "User must login first")
        {
        }
    }
}
