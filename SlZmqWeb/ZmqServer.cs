using System;
using System.Threading;
using ZeroMQ;

namespace SilverlightChatHub
{
    /// <summary>
    /// Simple request/response server
    /// </summary>
    internal sealed class ZmqServer : IDisposable
    {
        private readonly string _endpoint = "tcp://*:5555";
        private readonly Thread _workerThread;
        private Func<string, string> _messageReceivedHandler;

        public ZmqServer()
        {
            _workerThread = new Thread(Run);
        }

        public void Start()
        {
            _workerThread.Start();
        }

        public void SetMessageReceivedHandler(Func<string, string> handler)
        {
            _messageReceivedHandler = handler;
        }

        public void Dispose()
        {
            _workerThread.Abort();
            Console.WriteLine("Waiting for server thread to close...");
            var i = 0;
            while (_workerThread.IsAlive)
            {
                Thread.Sleep(100);
                if (i++ > 30)
                    break;
            }
            Console.WriteLine("Thread still alive...meh. Bye.");
        }

        private void Run()
        {
            try
            {
                using (var context = new ZContext())
                using (var responder = new ZSocket(context, ZSocketType.REP))
                {
                    responder.Bind(_endpoint);

                    while (true)
                    {
                        using (var request = responder.ReceiveFrame())
                        {
                            var response = _messageReceivedHandler?.Invoke(request.ToString());
                            responder.Send(new ZFrame(response));
                        }
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("close requested, server closing");
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("close requested, server closing");
            }
        }
    }
}