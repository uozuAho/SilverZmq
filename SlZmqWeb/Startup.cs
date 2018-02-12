using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SilverlightChatHub.Startup))]

namespace SilverlightChatHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
