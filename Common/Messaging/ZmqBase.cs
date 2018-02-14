using System;
using ZeroMQ;

namespace Common.Messaging
{
    internal abstract class ZmqBase : IMessageQueue
    {
        protected readonly string _endpoint;
        private ZSocket _zSocket;
        private ZContext _zContext;

        protected ZmqBase(string endpoint)
        {
            _endpoint = endpoint;
        }

        public virtual string Read()
        {
            if (!IsConnected)
                throw new InvalidOperationException("not connected");
            return _zSocket.ReceiveFrame().ToString();
        }

        public virtual void Write(string message)
        {
            if (!IsConnected)
                throw new InvalidOperationException("not connected");
            _zSocket.Send(new ZFrame(message));
        }

        public bool IsConnected { get; private set; } = false;

        public void Connect()
        {
            if (IsConnected) throw new InvalidOperationException("already connected");
            _zContext = new ZContext();
            _zSocket = CreateSocket(_zContext);
            ConnectSocket(_zSocket);
            IsConnected = true;
        }

        public void Dispose()
        {
            _zContext?.Dispose();
            _zSocket?.Dispose();
        }

        protected abstract ZSocket CreateSocket(ZContext context);

        protected abstract void ConnectSocket(ZSocket socket);
    }
}
