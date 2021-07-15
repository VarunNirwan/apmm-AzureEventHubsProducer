using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.CustomExceptions
{
    public class InvalidPayloadException : BaseException
    {
        public InvalidPayloadException(string message) : base(message)
        {

        }
    }
}
