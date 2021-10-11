using System;
using System.Net;
using Newtonsoft.Json;

namespace NIO.DefaultException
{
    public class DefaultResponse
    {
         [JsonProperty("error")]
        public String Error { get; set; } = HttpStatusCode.InternalServerError.ToString();

        [JsonProperty("exception")]
        public String Exception { get; set; }

        [JsonProperty("message")]
        public String Message { get; set; }
        
        [JsonProperty("stackTrace")]
        public String StackTrace { get; set; }

        [JsonProperty("path")]
        public String Path { get; set; }

        [JsonProperty("status")]
        public Int32 Status { get; set; }

        [JsonProperty("timestamp")]
        public Int32 Timestamp { get; set; } = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}