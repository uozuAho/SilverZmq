using System;
using Common.Messaging;
using SilverlightChatHub.Messaging;

namespace ZmqConsoleClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const int sendPort = ChatHub.RecvPort;
            const int recvPort = ChatHub.SendPort;

            var closeRequested = false;
            var queueFactory = new MessageQueueFactory();
            Console.CancelKeyPress += (e, a) =>
            {
                closeRequested = true;
            };

            using (var sendQueue = queueFactory.GetWriteOnlyQueue("tcp://127.0.0.1:" + sendPort))
            using (var recvQueue = queueFactory.GetReadOnlyQueue("tcp://127.0.0.1:" + recvPort))
            {
                sendQueue.Connect();
                recvQueue.Connect();
                while (!closeRequested)
                {
                    Console.WriteLine("enter a message");
                    var input = Console.ReadLine();
                    sendQueue.Write(input);
                    while (true)
                    {
                        var response = recvQueue.Read();
                        Console.WriteLine("response: " + response);
                        if (response == "END")
                            break;
                    }
                }
                Console.WriteLine("Shutting down...");
            }
        }
    }
}
