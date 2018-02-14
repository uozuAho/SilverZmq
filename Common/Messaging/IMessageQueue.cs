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
    }
}
