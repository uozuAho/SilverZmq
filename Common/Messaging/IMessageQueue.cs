using System;

namespace Common.Messaging
{
    public interface IMessageQueue : IDisposable
    {
        string Read();
        void Write(string message);
    }
}
