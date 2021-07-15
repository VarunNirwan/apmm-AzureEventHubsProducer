using AzureEventHubs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Repository
{
    public interface IUserRepository
    {
        Task SendAsync(User emp);
        string HelpText();
    }
}
