using System;
using CygniAPI;
using Newtonsoft.Json;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var _ = new Application();
        }
    }

    class Application
    {
        public Application()
        {
            var config = new CygniConfiguration{ ListeningPath = "localhost", ListeningPort = 8080 };
            var server = new CygniApiServer(config);

            // Register several different requests
            Console.WriteLine($"http://{config.ListeningPath}:{config.ListeningPort}/hello\t\tsends a greeting message to the screen.");
            server.Get("/sayhello", (i, o) =>
            {
                Console.WriteLine("The page says hello.");
                o.Append("Hello.");
            });

            Console.WriteLine($"http://{config.ListeningPath}:{config.ListeningPort}/date\t\tdisplays the current date time n the page.");
            server.Get("/date", (i, o) =>
            {
                Console.WriteLine("The page displays current date in UTC.");
                o.Append(DateTime.UtcNow);
            });

            Console.WriteLine($"http://{config.ListeningPath}:{config.ListeningPort}/config\t\tdisplays the current configuration of the server.");
            server.Get("/config", (i, o) =>
            {
                Console.WriteLine("The page displays server config.");
                o.Append(JsonConvert.SerializeObject(config));
            });

            Console.WriteLine($"http://{config.ListeningPath}:{config.ListeningPort}/self\t\tdisplays the current request class in JSON.");
            server.Get("/self", (i, o) =>
            {
                Console.WriteLine("The page displays self request.");
                var newContext = i;
                o.Append(JsonConvert.SerializeObject(newContext));
            });

            server.Start();

            Console.WriteLine("Press space to stop...\n\n");

            var result = 0;
            while(result != (int)ConsoleKey.Spacebar)
            {
                result = (int)Console.ReadKey().Key;
            }

            server.Stop();
            server = null;
            GC.Collect();
        }
    }
}
