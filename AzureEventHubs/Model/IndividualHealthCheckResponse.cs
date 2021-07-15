using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Model
{
    public class IndividualHealthCheckResponse
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
