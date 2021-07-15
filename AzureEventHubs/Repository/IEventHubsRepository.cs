using AzureEventHubs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Repository
{
    public interface IEventHubsRepository
    {
        Task SendAsync(Employees employees);
        string HelpText();
    }
}
