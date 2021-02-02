using System;
using System.Text;
using System.Text.Json;
using Dapr.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BankAuditor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDaprClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, DaprClient daprClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudEvents();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapPost("/history", async context =>
                {
                    var history = await context.Request.ReadFromJsonAsync<TransactionHistory>();
                    if (history.StartingBalance - history.EndingBalance >= 200m)
                    {
                        logger.LogInformation(
                            "Whoa there {Username} is a big spender! Before: {StartingBalance} After:{EndingBalance}", 
                            history.Username,
                            history.StartingBalance,
                            history.EndingBalance);

                        // TODO - keep a record of all the big transactions
                        var request = new BindingRequest("auditstore", "create")
                        {
                            Data = CreateUpload(history),
                        };

                        await daprClient.InvokeBindingAsync(request);
                    }

                    // Returning a 2XX status code will mark the message as 'read'
                    //
                    // This would be the default, so it's not explicitly necessary to put in code.
                    context.Response.StatusCode = 200; 
                }).WithTopic("pubsub", "history");
            });
        }

        private byte[] CreateUpload(TransactionHistory history)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(history, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var encoded = "\"" + Convert.ToBase64String(bytes) + "\"";
            return Encoding.UTF8.GetBytes(encoded);
        }
    }
}
