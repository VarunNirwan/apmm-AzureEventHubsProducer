using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.CustomExceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(string message) : base(message)
        {

        }
    }
}
