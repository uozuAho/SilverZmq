using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SilverlightChatHub
{
    public class ChatHub : Hub
    {
        private static ZmqServer _zmqServer;
        
        // not sure if concurrent queue is necessary...
        private static ConcurrentQueue<string> _clientResponses;
        private static ConcurrentQueue<string> ClientResponses =>
            _clientResponses ?? (_clientResponses = new ConcurrentQueue<string>());

        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
            EnsureZmqServerRunning();
        }

        public void SendToConsole(string message)
        {
            ClientResponses.Enqueue(message);
        }

        private string ProcessConsoleRequest(string message)
        {
            Clients.All.consoleBroadcast(message);
            // there's probably a better async way to do this...
            for (var i = 0; i < 10; i++)
            {
                if (ClientResponses.Count <= 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                string result;
                return ClientResponses.TryDequeue(out result)
                    ? result 
                    : "failed to read from queue????";
            }
            return "timed out waiting for response";
        }

        public override Task OnConnected()
        {
            EnsureZmqServerRunning();
            return Task.FromResult(0);
        }

        private void EnsureZmqServerRunning()
        {
            if (_zmqServer != null) return;
            _zmqServer = CreateZmqServer();
            _zmqServer.Start();
        }

        private ZmqServer CreateZmqServer()
        {
            var server = new ZmqServer();
            server.SetMessageReceivedHandler(ProcessConsoleRequest);
            return server;
        }
    }
}