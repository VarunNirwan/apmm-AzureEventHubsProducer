using AzureEventHubs.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureEventHubs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[CustomExceptionFilter]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Wish(string id)
        {
            //throw new CustomExceptions.BadRequestException("Error thrown.");

            int i = Convert.ToInt32(id);
            return Ok(id); ;
        }

        //[HttpGet]
        //public IActionResult Post(int id)
        //{

        //    return Ok("Succes"+id.ToString());
        //}
    }
}
