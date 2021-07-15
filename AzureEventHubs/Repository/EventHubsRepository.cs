using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using AzureEventHubs.CustomExceptions;
using AzureEventHubs.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Repository
{
    public class EventHubsRepository : IEventHubsRepository
    {
        string connectionString;
        string eventHubName;
        private const string helpXML = @"Inorder to POST your data to the API, use below format of XML in Request Body.
                                
                                 <?xml version=""1.0"" encoding=""UTF-8""?>
                                 <Employees>
                                    <EmpDetails>
	                                    <Address>Bhubaneswar</Address>
	                                    <Age>32</Age>
                                    </EmpDetails>
                                    <EmpName>
	                                    <FirstName>Kumar Debasis</FirstName>
	                                    <LastName>Barik</LastName>
                                    </EmpName>
                                 </Employees>";
        private readonly ILogger<EventHubsRepository> _logger;

        public EventHubsRepository(ILogger<EventHubsRepository> logger)
        {
            connectionString = ConfigurationManager.AppSetting["ConnectionStrings:EventHubConnectionString"];
            eventHubName = ConfigurationManager.AppSetting["ConnectionStrings:EventHubName"];
            this._logger = logger;
        }

        public string HelpText()
        {
            return helpXML;
        }

        public async Task SendAsync(Employees employees)
        {
            _logger.LogInformation("SendAsync invoked.");
            //Building options for retry
            EventHubProducerClientOptions options = new EventHubProducerClientOptions();
            options.RetryOptions.MaximumRetries = 10;
            options.RetryOptions.MaximumDelay = TimeSpan.FromMinutes(1);

            /*EventHubsRetryOptions retryOptions = new EventHubsRetryOptions();
            retryOptions.MaximumRetries = 10;
            retryOptions.MaximumDelay = TimeSpan.FromMinutes(1);
            options.RetryOptions = retryOptions;*/

            //Building ConnectionString for zure EventHubs
            var producer = new EventHubProducerClient(connectionString, eventHubName, options);

            using var eventBatch = await producer.CreateBatchAsync();
            var eventBody = new BinaryData(ParseEmployee(employees));
            var eventData = new EventData(eventBody);

            //Adding event data to event batch
            if (!eventBatch.TryAdd(eventData))
            {
                _logger.LogError("Failed to add event to the EventHubs.");
                throw new NotFoundException($"The event could not be added.");
            }

            //Sending event data batch to Azure EventHubs
            await producer.SendAsync(eventBatch);
        }

        private List<Employees> ParseEmployee(Employees emp)
        {
            var empDetails = new Employees
            {
                EmpDetails = new Employees.Employee
                {
                    FullName = emp.EmpName.FirstName + " " + emp.EmpName.LastName, //emp.EmpDetails.FullName,
                    Address = emp.EmpDetails.Address,
                    Age = emp.EmpDetails.Age
                },
                EmpName = new Employees.Names
                {
                    FirstName = emp.EmpName.FirstName,
                    LastName = emp.EmpName.LastName
                },
            };

            List<Employees> employees = new List<Employees>();
            employees.Add(empDetails);

            return employees;
        }
    }
}
