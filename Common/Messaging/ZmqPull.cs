using System;
using ZeroMQ;

namespace Common.Messaging
{
    internal class ZmqPull : IMessageQueue
    {
        private readonly ZContext _zContext;
        private readonly ZSocket _zSocket;

        public ZmqPull(string endpoint)
        {
            _zContext = new ZContext();
            _zSocket = new ZSocket(_zContext, ZSocketType.PULL);
            _zSocket.Bind(endpoint);
        }

        public string Read()
        {
            return _zSocket.ReceiveFrame().ToString();
        }

        public void Write(string message)
        {
            throw new InvalidOperationException("This queue is read-only");
        }

        public void Dispose()
        {
            _zContext?.Dispose();
            _zSocket?.Dispose();
        }
    }
}
