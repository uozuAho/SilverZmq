using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SilverlightChatHub.Messaging
{
    public class ChatHub : Hub
    {
        private static CommandServer _commandServer;

        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
            EnsureCommandServerRunning();
            _commandServer.SendMessage(message);
        }

        public void SendToConsole(string message)
        {
            EnsureCommandServerRunning();
            _commandServer.SendMessage(message);
        }

        public override Task OnConnected()
        {
            EnsureCommandServerRunning();
            return Task.FromResult(0);
        }

        private void ProcessConsoleRequest(string message)
        {
            Clients.All.consoleBroadcast(message);
        }

        private void EnsureCommandServerRunning()
        {
            if (_commandServer == null)
                _commandServer = CreateCommandServer();
        }

        private CommandServer CreateCommandServer()
        {
            var server = new CommandServer("tcp://127.0.0.1:5556", "tcp://127.0.0.1:5555");
            server.SetCommandReceivedHandler(ProcessConsoleRequest);
            return server;
        }
    }
}