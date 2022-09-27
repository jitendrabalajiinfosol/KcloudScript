using System;
using System.Net;

namespace KcloudScript.Model
{
    public class ResponseEntity
    {
        public HttpStatusCode statusCode { get; set; }
        public bool result { get; set; }
        public object data { get; set; }
        public string message { get; set; }

        public ResponseEntity()
        {

        }
        public ResponseEntity(HttpStatusCode _statusCode, bool _result, object _data, string _message)
        {
            statusCode = _statusCode;
            result = _result;
            data = _data;
            message = _message;
        }
    }
}
