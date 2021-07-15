using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AzureEventHubs.Errors
{
    public class Error
    {
        public string message { get; set; }
        public bool isError { get; set; }
        //public string detail { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string stack { get; set; }

        public Error(string _message)
        {
            this.message = _message;
            isError = true;
        }
    }
}
