using System;

namespace Common.Messaging
{
    public interface IMessageQueue : IDisposable
    {
        /// <summary>
        /// Get a message from the queue. May block or return
        /// null, depending on the queue implementation.
        /// </summary>
        string Read();

        /// <summary>
        /// Write a message to the queue.
        /// </summary>
        void Write(string message);

        /// <summary>
        /// Open the connection. This may entail 'binding' (waiting
        /// for a connection) or actually connecting to a bound port.
        /// </summary>
        void Connect();

        /// <summary>
        /// Is connected and can send/receive messages
        /// </summary>
        bool IsConnected { get; }
    }
}
