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
        // outbox to command client
        private readonly IMessageQueue _sendQueue;
        // inbox from command client
        private readonly IMessageQueue _recvQueue;

        private readonly Thread _listenerThread;

        private Action<string> _onCommandReceived;

        /// <summary>
        /// Create a command server
        /// </summary>
        /// <param name="sendEndpoint">Endpoint that the server sends messages to</param>
        /// <param name="recvEndpoint">Endpoint that the server receives commands on</param>
        public CommandServer(string sendEndpoint, string recvEndpoint)
        {
            var messageQueueFactory = new MessageQueueFactory();

            _sendQueue = messageQueueFactory.GetWriteOnlyQueue(sendEndpoint);
            _recvQueue = messageQueueFactory.GetReadOnlyQueue(recvEndpoint);
            // receive queue can connect straight away since it waits for incoming
            // connections
            _recvQueue.Connect();
            _listenerThread = new Thread(ListenerLoop);
            _listenerThread.Start();
        }

        // I should learn how to use events...
        /// <summary>
        /// Set handler for when messages are received from a command client
        /// </summary>
        public void SetCommandReceivedHandler(Action<string> handler)
        {
            _onCommandReceived = handler;
        }

        public void SendMessage(string message)
        {
            // Connecting to the command client is delayed until a 
            // message is received from the client, otherwise connection
            // fails and we send messages into the ether
            if (!_sendQueue.IsConnected)
                _sendQueue.Connect();
            _sendQueue.Write(message);
        }

        private void ListenerLoop()
        {
            try
            {
                while (true)
                {
                    var message = _recvQueue.Read();
                    _onCommandReceived?.Invoke(message);
                    // is this needed to be able to be interrupted?
                    Thread.Sleep(1);
                }
            }
            catch (ThreadInterruptedException) {}
        }

        public void Dispose()
        {
            _sendQueue?.Dispose();
            _recvQueue?.Dispose();
            _listenerThread.Interrupt();
        }
    }
}
