using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace weatherapp_route_to_code
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var uri = new Uri(Environment.GetEnvironmentVariable("FORECAST_SERVICE_URI") ?? "http://localhost:8080/");
                logger.LogInformation("Using {URI} for forecast service", uri);

                var client = new HttpClient()
                {
                    BaseAddress = uri,
                };

                endpoints.MapGet("/", async context =>
                {
                    var forecast = await client.GetFromJsonAsync<WeatherForecast>("/forecast");
                    var report = new WeatherReport()
                    {
                        Location = "Seattle",
                        Forecast = forecast.Weather,
                    };

                    await context.Response.WriteJsonAsync(report);
                });
            });
        }
    }
}
