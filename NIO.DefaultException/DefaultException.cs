using System;
using System.Net;
using System.Runtime.Serialization;

namespace NIO.DefaultException
{
    [Serializable]
    public class DefaultException : Exception
    {
        public DefaultException()
        {
            this.Status = HttpStatusCode.BadRequest;
        }

        public DefaultException(String message) : base(message)
        {
            this.Status = HttpStatusCode.BadRequest;
        }

        public DefaultException(String message, Exception innerException) : base(message, innerException)
        {
            this.Status = HttpStatusCode.BadRequest;
        }

        protected DefaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Status = HttpStatusCode.BadRequest;
        }

        public HttpStatusCode Status { get; set; }
    }
}
