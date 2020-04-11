using System;
using Echo;
using static Echo.Process;

namespace EchoServer
{
    internal static class Program
    {
        private static void Main()
        {
            RedisCluster.register();
            ProcessConfig.initialise("test-cluster", "main-server", "node-1", "localhost", "0");

            ProcessSystemLog.Subscribe(Console.WriteLine);

            var logger = spawn<int, string>(
                "logger",
                () => 0,
                (count, message) =>
                {
                    Console.WriteLine("--- MESSAGE RECEIVED ---");
                    Console.WriteLine($"{message} - {DateTime.Now:HH:mm:ss.fff}");
                    Console.WriteLine("--- MESSAGE HANDLED ---");
                    Console.WriteLine("--- META INFO START ---");
                    Console.WriteLine(Self);
                    Console.WriteLine(Parent);
                    Console.WriteLine(Systems);
                    Console.WriteLine(Children);
                    Console.WriteLine(Sender);
                    Console.WriteLine(isAsk);
                    Console.WriteLine(isTell);
                    Console.WriteLine(inboxCount(Self));
                    Console.WriteLine(Role.NodeIds(Self).ToSeq());
                    Console.WriteLine(Role.NodeIds(Sender).ToSeq());
                    foreach (var systemName in Systems)
                    {
                        Console.WriteLine(DeadLetters(systemName));
                        Console.WriteLine(Errors(systemName));
                        Console.WriteLine(Root(systemName));
                        Console.WriteLine(User(systemName));
                        Console.WriteLine(getSupplementarySessionId());
                        Console.WriteLine(hasActiveSession());
                        Console.WriteLine(hasSession());
                        Console.WriteLine(ClusterNodes(systemName).Map(cn => (cn.Role, cn.NodeName, cn.LastHeartbeat)));
                        Console.WriteLine(Role.Nodes(Self, systemName));
                        Console.WriteLine(Role.Nodes(Sender, systemName));
                    }

                    Console.WriteLine("--- META INFO END ---");

                    return count + 1;
                },
                ProcessFlags.PersistAll);

            register("logger", logger);

            Console.WriteLine(logger);

            Console.ReadLine();
        }
    }
}
