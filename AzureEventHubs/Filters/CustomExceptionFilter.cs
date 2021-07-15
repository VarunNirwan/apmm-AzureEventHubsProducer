using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureEventHubs.CustomExceptions;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using AzureEventHubs.Errors;
using Microsoft.Extensions.Logging;

namespace AzureEventHubs.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        ILogger logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> _logger)
        {
            logger = _logger;
            logger.LogInformation("From CustomExceptionFilter");
        }
        /*
        public override void OnException(ExceptionContext context)
        {
            Error error = new Error("");
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errorMessage = string.Empty;

            if (context.Exception is BadRequestException || context.Exception is InvalidPayloadException)
            {
                statusCode = HttpStatusCode.BadRequest;
                error.message = "Error: Bad Request or Invalid Payload.";
            }
            else if(context.Exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                error.message = "Error: Unauthorized Access.";
            }
            else
            {
                error.message = "Internal Server Error. Please contact Application Team.";
            }
            error.statusCode = statusCode;
            error.stack = context.Exception.StackTrace;

            context.Result = new JsonResult(error);
            base.OnException(context);
        }
        */

        //Add log
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            Error error = new Error("");
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errorMessage = string.Empty;

            var task = base.OnExceptionAsync(context);
           
            return task.ContinueWith(
                t =>
                {
                    if (context.Exception is BadRequestException || context.Exception is InvalidPayloadException)
                    {
                        statusCode = HttpStatusCode.BadRequest;
                        error.message = "Error: Bad Request or Invalid Payload.";
                    }
                    else if (context.Exception is UnauthorizedAccessException)
                    {
                        statusCode = HttpStatusCode.Unauthorized;
                        error.message = "Error: Unauthorized Access.";
                    }
                    else
                    {
                        error.message = "Internal Server Error. Please contact Application Team.";
                    }
                    error.statusCode = statusCode;
                    error.stack = context.Exception.StackTrace;

                    logger.LogError(context.Exception, error.message);

                    context.Result = new JsonResult(error);
                    base.OnExceptionAsync(context);
                });
        }

    }
}
