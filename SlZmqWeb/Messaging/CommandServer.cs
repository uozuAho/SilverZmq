using System;
using System.Threading;
using Common.Messaging;

namespace SilverlightChatHub.Messaging
{
    /// <summary>
    /// Server for receiving commands to pass to the silverlight
    /// client, and returning responses.
    /// </summary>
    internal sealed class CommandServer
    {
        private readonly IMessageQueue _pushQueue;
        private readonly IMessageQueue _pullQueue;

        private readonly Thread _listenerThread;

        private Action<string> _onCommandReceived;

        public CommandServer(string pushEndpoint, string pullEndpoint)
        {
            var messageQueueFactory = new MessageQueueFactory();

            _pushQueue = messageQueueFactory.GetWriteOnlyQueue(pushEndpoint);
            _pullQueue = messageQueueFactory.GetReadOnlyQueue(pullEndpoint);
            _listenerThread = new Thread(ListenerLoop);
            _listenerThread.Start();
        }

        // I should learn how to use events...
        public void SetCommandReceivedHandler(Action<string> handler)
        {
            _onCommandReceived = handler;
        }

        public void SendMessage(string message)
        {
            _pushQueue.Write(message);
        }

        private void ListenerLoop()
        {
            try
            {
                while (true)
                {
                    var message = _pullQueue.Read();
                    _onCommandReceived?.Invoke(message);
                    // is this needed to be able to be interrupted?
                    Thread.Sleep(1);
                }
            }
            catch (ThreadInterruptedException) {}
        }

        public void Dispose()
        {
            _pushQueue?.Dispose();
            _pullQueue?.Dispose();
            _listenerThread.Interrupt();
        }
    }
}
