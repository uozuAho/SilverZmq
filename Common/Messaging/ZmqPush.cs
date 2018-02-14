using System;
using ZeroMQ;

namespace Common.Messaging
{
    internal sealed class ZmqPush : IMessageQueue
    {
        private readonly ZContext _zContext;
        private readonly ZSocket _zSocket;

        public ZmqPush(string endpoint)
        {
            _zContext = new ZContext();
            _zSocket = new ZSocket(_zContext, ZSocketType.PUSH);
            _zSocket.Connect(endpoint);
        }

        public string Read()
        {
            throw new InvalidOperationException("This queue is write-only");
        }

        public void Write(string message)
        {
            _zSocket.Send(new ZFrame(message));
        }

        public void Dispose()
        {
            _zContext?.Dispose();
            _zSocket?.Dispose();
        }
    }
}
