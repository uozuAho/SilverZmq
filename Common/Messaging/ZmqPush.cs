using System;
using ZeroMQ;

namespace Common.Messaging
{
    internal sealed class ZmqPush : ZmqBase
    {
        public ZmqPush(string endpoint) : base(endpoint)
        {
        }

        public override string Read()
        {
            throw new InvalidOperationException("This queue is write-only");
        }

        protected override ZSocket CreateSocket(ZContext context)
        {
            return new ZSocket(context, ZSocketType.PUSH);
        }

        protected override void ConnectSocket(ZSocket socket)
        {
            socket.Connect(_endpoint);
        }
    }
}
