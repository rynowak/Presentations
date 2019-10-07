using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace SignalRSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Giving Kestrel some additional instructions
                    webBuilder.ConfigureKestrel(kestrel =>
                    {
                        // Listen normally (do HTTP and run middleware)
                        kestrel.Listen(IPAddress.Loopback, 5000); 

                        // Listen and hand it to SignalR directly
                        kestrel.Listen(IPAddress.Loopback, 5006, builder => builder.UseHub<Chat>());
                    });
                });
    }
}
