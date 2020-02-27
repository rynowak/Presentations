using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace trainingday_backend
{
    public class Startup
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var connection = context.Features.Get<IHttpConnectionFeature>();
                    var backendInfo = new BackendInfo()
                    {
                        IP = connection.LocalIpAddress.ToString(),
                        Hostname = Dns.GetHostName(),
                    };

                    context.Response.ContentType = "application/json; charset=utf-8";
                    await JsonSerializer.SerializeAsync(context.Response.Body, backendInfo);
                });

                endpoints.MapGet("/crash", context =>
                {
                    System.Environment.Exit(1);
                    return Task.CompletedTask;
                });

                endpoints.MapHealthChecks("/healthz");
            });
        }

        class BackendInfo
        {
            public string IP { get; set; }

            public string Hostname { get; set; }
        }
    }
}
