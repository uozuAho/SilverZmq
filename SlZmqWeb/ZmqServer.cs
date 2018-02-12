using System;
using System.Threading;
using ZeroMQ;

namespace SilverlightChatHub
{
    internal sealed class ZmqServer : IDisposable
    {
        private readonly string _endpoint = "tcp://*:5555";

        private readonly Thread _workerThread;

        // todo: onReceive event handler

        public ZmqServer()
        {
            _workerThread = new Thread(() => Run(_endpoint));
        }

        public void Start()
        {
            _workerThread.Start();
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

        private static void Run(string endpoint)
        {
            try
            {
                using (var context = new ZContext())
                using (var responder = new ZSocket(context, ZSocketType.REP))
                {
                    responder.Bind(endpoint);

                    while (true)
                    {
                        using (var request = responder.ReceiveFrame())
                        {
                            Console.WriteLine("Received {0}", request.ReadString());
                            responder.Send(new ZFrame("tally ho"));
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("close requested, server closing");
            }
        }
    }
}