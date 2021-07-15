using AzureEventHubs.Model;
using AzureEventHubs.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetHelp()
        {
            return Ok(userRepository.HelpText());
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] User user)
        {
            logger.LogInformation("PostData invoked.");
            if (user != null)
            {
                logger.LogInformation("Sending data to EventHubs.");
                await userRepository.SendAsync(user);
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
