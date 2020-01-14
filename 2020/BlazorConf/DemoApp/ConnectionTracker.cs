using System.Threading;

namespace DemoApp
{
    class ConnectionTracker
    {
        private int activeConnections;

        public int ActiveConnections => activeConnections;

        public void Increment()
        {
            Interlocked.Increment(ref activeConnections);
        }

        public void Decrement()
        {
            Interlocked.Decrement(ref activeConnections);
        }
    }
}
