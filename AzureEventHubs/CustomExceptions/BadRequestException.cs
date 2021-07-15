using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.CustomExceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
