using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DemoApp
{
    class CounterCircuitHandler : CircuitHandler
    {
        private readonly ConnectionTracker tracker;
        private readonly ConcurrentDictionary<string, bool> ids;

        public CounterCircuitHandler(ConnectionTracker tracker)
        {
            this.tracker = tracker;
            this.ids = new ConcurrentDictionary<string, bool>();
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            if (ids.TryAdd(circuit.Id, true))
            {
                tracker.Increment();
            }

            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            if (ids.TryRemove(circuit.Id, out _))
            {
                tracker.Decrement();
            }

            return Task.CompletedTask;
        }
    }
}
