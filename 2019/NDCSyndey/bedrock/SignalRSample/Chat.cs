using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample
{
    public class Chat : Hub
    {
        public Task Send(string user, string message)
        {
            return Clients.All.SendAsync("Send", user, message);
        }
    }
}
