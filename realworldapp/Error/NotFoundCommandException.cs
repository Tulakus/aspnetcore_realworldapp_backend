using System.Net;

namespace realworldapp.Error
{
    public class NotFoundCommandException : HttpCommandException
    {
        public NotFoundCommandException(object message): base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
