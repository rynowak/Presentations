using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public class Application
    {
        public IConfiguration Configuration => Services?.GetRequiredService<IConfiguration>();

        public IWebHostEnvironment Env => Services?.GetRequiredService<IWebHostEnvironment>();

        public IServiceProvider Services { get; set; }

        public static void Run<TStartup>(string[] args) where TStartup : Application, new()
        {
            CreateHostBuilder<TStartup>(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : Application, new()
        {
            var builder = Host.CreateDefaultBuilder(args);

            var startup = new TStartup();
            builder.ConfigureWebHostDefaults(b =>
            {
                b.ConfigureServices(startup.ConfigureServices);
                b.Configure((context, app) =>
                {
                    startup.Services = app.ApplicationServices;
                    startup.Configure(app);
                });
            });

            return builder;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public virtual void Configure(IApplicationBuilder app)
        { 
        }
    }
}
