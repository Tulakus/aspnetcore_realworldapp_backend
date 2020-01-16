using System.Net;

namespace realworldapp.Error
{
    public class UnauthorizedCommandException : HttpCommandException
    {
        public UnauthorizedCommandException() : base(HttpStatusCode.Unauthorized, new
        {
            User = "You must login first"
        })

        {
        }
    }
}
