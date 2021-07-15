using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using AzureEventHubs.CustomExceptions;
using AzureEventHubs.Filters;
using AzureEventHubs.Model;
using AzureEventHubs.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AzureEventHubs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[CustomExceptionFilter]
    public class SenderController : ControllerBase
    {
        private readonly ILogger<SenderController> logger;
        private readonly IEventHubsRepository eventHubsRepository;

        public SenderController(ILogger<SenderController> logger, IEventHubsRepository eventHubsRepository)
        {
            this.logger = logger;
            this.eventHubsRepository = eventHubsRepository;
        }

        public IActionResult GetHelp()
        {
            return Ok(eventHubsRepository.HelpText());
        }

        [HttpPost]
        //[ServiceFilter(typeof(CustomExceptionFilter))]
        public async Task<IActionResult> PostData([FromBody] Employees emp)
        {
            logger.LogInformation("PostData invoked.");
            if (emp != null)
            {
                logger.LogInformation("Sending data to EventHubs.");
                await eventHubsRepository.SendAsync(emp);
                logger.LogInformation("Data Sent to EventHubs.");
                return Ok("Success!");
            }
            else
            {
                logger.LogInformation("Input XML not found.");
                return NotFound("User data not found");
            }
        }

    }
}
