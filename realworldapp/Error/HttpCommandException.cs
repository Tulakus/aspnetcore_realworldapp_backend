using System;
using System.Net;

namespace realworldapp.Error
{
    public class HttpCommandException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public new object Message { get; set; }
        public HttpCommandException(HttpStatusCode statusCode, object message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
