using AzureEventHubs.Middlewares;
using AzureEventHubs.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureEventHubs.Repository;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using AzureEventHubs.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AzureEventHubs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //----------------------Custom Services----------------------
            services.AddScoped<IEventHubsRepository, EventHubsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //----------------------Exception Middleware----------------------
            services.AddTransient<CustomExceptionMiddleware>();

            //----------------------AppInsights service registration----------------------
            services.AddApplicationInsightsTelemetry();

            //----------------------JWT Authentication----------------------
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Audience = Configuration["AAD:ResourceId"];
                    opt.Authority = $"{Configuration["AAD:Instance"]}{Configuration["AAD:TenantId"]}";
                });

            //----------------------Controller Registration----------------------
            services.AddControllers().AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();

            //----------------------Healthchecks----------------------
            services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://codewithmukesh.com"), name: "CodewithMukesh")
                .AddSqlServer(Configuration["ConnectionStrings:dbConnectionString"], tags: new[] {
                "db",
                "all"
            }).AddAzureBlobStorage(Configuration["ConnectionStrings:blobConnectionString"], tags: new[] {
                "AzureStorage",
                "all"
            }).AddAzureQueueStorage(Configuration["ConnectionStrings:QueueConnectionString"], tags: new[] {
                "AzureStorage",
                "all"
            })
            .AddAzureEventHub(Configuration["ConnectionStrings:EventHubConnectionString"], Configuration["ConnectionStrings:EventHubName"], tags: new[] {
                "AzureEventHub",
                "all"
            });

            #region Registering CustomExceptionFilter service
            //Registering CustomExceptionFilter service
            /*services.AddTransient<CustomExceptionFilter>();
            services.AddControllers(options =>
            {
                //Adding CustomExceptionFilter to the controllers filtr collection.
                options.Filters.AddService<CustomExceptionFilter>();
            });*/
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //Use ExceptionMiddleware
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            //HealthChecks
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckReponse
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse()
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
