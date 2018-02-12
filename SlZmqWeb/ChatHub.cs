using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SilverlightChatHub
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        public override Task OnConnected()
        {
            return 0;
        }
    }
}