using System.Net;

namespace realworldapp.Error
{
    public class ValidationCommandException: HttpCommandException
    {
        public ValidationCommandException(object message): base(HttpStatusCode.UnprocessableEntity, message)
        {
        }
    }
}
