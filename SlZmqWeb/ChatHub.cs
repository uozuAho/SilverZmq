using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SilverlightChatHub
{
    public class ChatHub : Hub
    {
        private static ZmqServer _zmqServer;

        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
            EnsureZmqServerRunning();
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

        private static ZmqServer CreateZmqServer()
        {
            var server = new ZmqServer();
            server.SetMessageReceivedHandler(req => "hello from ChatHub!");
            return server;
        }
    }
}