using System;
using System.Threading;
using Echo;
using LanguageExt;
using static Echo.Process;

namespace EchoServer
{
    internal static class Program
    {
        private static void Main()
        {
            RedisCluster.register();
            ProcessConfig.initialise("test-cluster", "main-server", "node-1", "localhost", "0");

            Console.Write("Waiting...");
            Console.ReadLine();

            ProcessId client = "//test-cluster/client-1/user/client-1";

            var t = spawn<Unit, string>("logger",
                () => subscribe(client),
                (unit, message) =>
                {
                    Console.WriteLine($"{message} - {DateTime.Now:HH:mm:ss.fff}");
                    return Unit.Default;
                });

            //register("logger", t);
            tell(t, "Start");

            Console.ReadLine();
        }
    }
}
