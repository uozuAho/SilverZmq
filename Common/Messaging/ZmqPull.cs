using System;
using ZeroMQ;

namespace Common.Messaging
{
    internal class ZmqPull : ZmqBase
    {
        public ZmqPull(string endpoint) : base(endpoint)
        {
        }

        public override void Write(string message)
        {
            throw new InvalidOperationException("This queue is read-only");
        }

        protected override ZSocket CreateSocket(ZContext context)
        {
            return new ZSocket(context, ZSocketType.PULL);
        }

        protected override void ConnectSocket(ZSocket socket)
        {
            socket.Bind(_endpoint);
        }
    }
}
