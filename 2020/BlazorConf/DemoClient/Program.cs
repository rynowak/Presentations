using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ignitor;
using Microsoft.AspNetCore.SignalR.Client;

namespace DemoClient
{
    class Program
    {
        async static Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: DemoClient <url> <count>");
                return 1;
            }

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.Write("Cancelling...");
                cts.Cancel();
            };

            var uri = new Uri(args[0]);
            var count = int.Parse(args[1]);

            var tasks = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine($"Creating client {i}...");
                var client = new BlazorClient()
                {
                    DefaultOperationTimeout = TimeSpan.FromSeconds(5),
                };

                while (true)
                {
                    try
                    {
                        var connected = await client.ConnectAsync(uri, connectAutomatically: true);
                        if (!connected)
                        {
                            throw new InvalidOperationException("Failed to connect.");
                        }

                        break;
                    }
                    catch (HttpRequestException ex) when (ex.Message.Contains("429"))
                    {
                        Console.WriteLine("Getting rate limited. Waiting...");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5));
                }

                tasks.Add(Task.Run(() => RunAsync(uri, client, cts.Token)));
            }

            Console.WriteLine($"Started {count} clients.");

            await Task.WhenAll(tasks);
            return 0;
        }

        async static Task RunAsync(Uri uri, BlazorClient client, CancellationToken cancellationToken)
        {
            var counterUri = uri.AbsoluteUri + "/counter";

            await using (client)
            {
                try
                {
                    await client.ExpectRenderBatch(async () =>
                    { 
                        await client.HubConnection.InvokeAsync("OnLocationChanged", $"{counterUri.ToString()}", false, cancellationToken);
                    });

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await client.ClickAsync("clicker");
                        await Task.Delay(TimeSpan.FromSeconds(0.5));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client failed: " + ex);
                }
            }
        }
    }
}
