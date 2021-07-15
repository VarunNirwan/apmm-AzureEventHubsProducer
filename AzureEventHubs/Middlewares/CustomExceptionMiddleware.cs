using AzureEventHubs.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AzureEventHubs.Middlewares
{
    public class CustomExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<CustomExceptionMiddleware> logger;

        public CustomExceptionMiddleware(ILogger<CustomExceptionMiddleware> logger)
        {
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                logger.LogInformation("Before - CustomExceptionMiddleware execution.");
                await next(context);
                logger.LogInformation("After - CustomExceptionMiddleware execution.");
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError("BadRequestException");
                await context.Response.WriteAsync(ex.Message);
            }
            catch (InvalidPayloadException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                logger.LogError("InvalidPayloadException");
                await context.Response.WriteAsync(ex.Message);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogError("NotFoundException");
                await context.Response.WriteAsync(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                logger.LogError("Unauthorized to consume this api");
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError("InternalServerError");
                await context.Response.WriteAsync(ex.Message);
            }
            //throw new NotImplementedException();
        }

        /*
         private HttpContext GetErrorAsync(ref HttpContext context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //await context.Response.WriteAsync(ex.Message);
            return context;
        }
        */
    }
}
