using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using Microsoft.AspNet.SignalR.Client;

namespace SilverlightSignalR
{
    public partial class MainPage : UserControl
    {
        private readonly IHubProxy _hub;
        private readonly HubConnection _connection;

        public MainPage()
        {
            InitializeComponent();

            button.Click += Button_Click;

            // init hub
            var serverUri = new Uri(HtmlPage.Document.DocumentUri, "/").ToString();
            _connection = new HubConnection(serverUri, true);
            _hub = _connection.CreateHubProxy("ChatHub");
            _hub.On<string, string>("broadcastMessage", (name, message) =>
            {
                Dispatcher.BeginInvoke(() => textBox.Text = name + ": " + message);
            });
            _connection.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.State == ConnectionState.Connected)
                _hub.Invoke("Send", "asdf", textBox.Text);
        }
    }
}
