using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithEndPoint(new IPEndPoint(IPAddress.Loopback, 5006))
                .Build();

            connection.On<string, string>("Send", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await connection.StartAsync();

            Console.WriteLine("Connected - type something and press enter.");
            while (true)
            {
                var message = Console.ReadLine();
                await connection.SendAsync("Send", "tcp", message);
            }
        }
    }
}
