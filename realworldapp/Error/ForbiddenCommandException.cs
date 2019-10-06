using System.Net;

namespace realworldapp.Error
{
    public class ForbiddenCommandException : HttpCommandException
    {
        public ForbiddenCommandException(object message): base(HttpStatusCode.Forbidden, "No privileges")
        {
        }
    }
}
