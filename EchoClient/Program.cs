using System;
using System.Threading;
using Echo;
using LanguageExt;
using static Echo.Process;
using static LanguageExt.Prelude;

namespace EchoClient
{
    internal static class Program
    {
        private static void Main()
        {
            RedisCluster.register();
            ProcessConfig.initialise("test-cluster", "client", "client-1", "localhost", "0");

            ProcessSystemLog.Subscribe(Console.WriteLine);

            var router = Router.roundRobin<string>(
                "router",
                new ProcessId[] { "@main-server:logger" },
                RouterOption.RemoveWorkerWhenTerminated);
            register("log-router", router);
            var i = 0;
            var sender = spawn<Unit>(
                "sender",
                _ =>
                {
                    while (true)
                    {
                        var message = $"sending {i}";
                        Console.WriteLine($"Sent message - {message}");
                        tell("@log-router", message);
                        i += 1;
                        Thread.Sleep(2000);
                    }
                });

            tell(sender, unit);

            Console.WriteLine(sender);

            Console.ReadLine();
        }
    }
}
