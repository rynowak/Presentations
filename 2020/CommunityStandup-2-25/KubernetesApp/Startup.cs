using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KubernetesApp
{
    public class Startup
    {
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
                    await context.Response.WriteAsync($"Listening IP: {connection.LocalIpAddress}{Environment.NewLine}");
                    await context.Response.WriteAsync($"Hostname: {Dns.GetHostName()}{Environment.NewLine}");
                    await context.Response.WriteAsync($"EnvVar Configuration value: {Configuration["App:Value"]}{Environment.NewLine}");
                });

                endpoints.MapGet("/crash", async context =>
                {
                    System.Environment.Exit(1);
                });

                endpoints.MapHealthChecks("/healthz");
            });
        }
    }
}
