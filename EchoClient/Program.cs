using System;
using System.Threading;
using Echo;
using LanguageExt;
using static Echo.Process;

namespace EchoClient
{
    internal static class Program
    {
        private static void Main()
        {
            RedisCluster.register();
            ProcessConfig.initialise("test-cluster", "client", "client-1", "localhost", "0");

            Console.Write("Waiting...");
            Console.ReadLine();

            ProcessId server = "//test-cluster/node-1/user/logger";

            var t = spawn<Unit>("client", _ => tell(server, "Hello World!"));

            while (true)
            {
                tell(t, Unit.Default);
                Thread.Sleep(500);
            }
        }
    }
}
