using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.CustomExceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
