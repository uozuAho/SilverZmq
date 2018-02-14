using System;
using Common.Messaging;

namespace ZmqConsoleClient
{
    internal class Program
    {
        public const int Port1 = 5555;
        public const int Port2 = 5556;

        public static void Main(string[] args)
        {
            var swap = args.Length == 1 && args[0] == "swap";
            var pushPort = swap ? Port1 : Port2;
            var pullPort = swap ? Port2 : Port1;

            var closeRequested = false;
            var queueFactory = new MessageQueueFactory();
            Console.CancelKeyPress += (e, a) =>
            {
                closeRequested = true;
            };

            Console.WriteLine("creating push channel...");
            var pushQueue = queueFactory.GetWriteOnlyQueue("tcp://127.0.0.1:" + pushPort);

            try
            {
                Console.WriteLine("creating pull channel...");
                var pullQueue = queueFactory.GetReadOnlyQueue("tcp://127.0.0.1:" + pullPort);
                try
                {
                    while (!closeRequested)
                    {
                        Console.WriteLine("enter a message");
                        var input = Console.ReadLine();
                        pushQueue.Write(input);
                        var response = pullQueue.Read();
                        Console.WriteLine("response: " + response);
//                        while (true)
//                        {
//                            Console.WriteLine("response: " + response);
//                            if (response == "END")
//                                break;
//                        }
                    }

                    Console.WriteLine("Shutting down...");
                }
                finally
                {
                    pullQueue?.Dispose();
                }
            }
            finally
            {
                pushQueue?.Dispose();
            }
        }
    }
}
