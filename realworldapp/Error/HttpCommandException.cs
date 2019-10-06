using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
