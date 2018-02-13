using System;
using ZeroMQ;

namespace ZmqConsoleClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                requester.Connect("tcp://127.0.0.1:5555");

                for (var n = 0; n < 10; ++n)
                {
                    const string requestText = "Hello";
                    Console.Write("Sending {0}…", requestText);

                    requester.Send(new ZFrame(requestText));

                    using (var reply = requester.ReceiveFrame())
                    {
                        Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
                    }
                }
            }
        }
    }
}
