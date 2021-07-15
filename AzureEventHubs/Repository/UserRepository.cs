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
    public class UserRepository : IUserRepository
    {
        string connectionString;
        string eventHubName;
        private const string helpXML = @"Inorder to POST your data to the API, use below format of XML in Request Body.
                                
                                 <?xml version=""1.0"" encoding=""UTF-8""?>
                                  <User>
	                                <id>1</id>
	                                <name>Ram</name>
	                                <pwd>password</pwd>
                                  </User>";
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger)
        {
            connectionString = ConfigurationManager.AppSetting["ConnectionStrings:EventHubConnectionString"];
            eventHubName = ConfigurationManager.AppSetting["ConnectionStrings:EventHubName"];
            this._logger = logger;
        }
        public string HelpText()
        {
            return helpXML;
        }
        public async Task SendAsync(User user)
        {
            _logger.LogInformation("SendAsync invoked.");
            //Building options for retry
            EventHubProducerClientOptions options = new EventHubProducerClientOptions();
            EventHubsRetryOptions retryOptions = new EventHubsRetryOptions();
            retryOptions.MaximumRetries = 10;
            retryOptions.MaximumDelay = TimeSpan.FromMinutes(1);
            options.RetryOptions = retryOptions;

            //Building ConnectionString for zure EventHubs
            var producer = new EventHubProducerClient(connectionString, eventHubName, options);

            using var eventBatch = await producer.CreateBatchAsync();
            var eventBody = new BinaryData(user);
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
    }
}
