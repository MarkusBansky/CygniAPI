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
            server.Get("/sayhello", (i, o) =>
            {
                Console.WriteLine("The page says hello.");
                o.Append("Hello.");
            });

            server.Get("/date", (i, o) =>
            {
                Console.WriteLine("The page displays current date in UTC.");
                o.Append(DateTime.UtcNow);
            });

            server.Get("/config", (i, o) =>
            {
                Console.WriteLine("The page displays server config.");
                o.Append(JsonConvert.SerializeObject(config));
            });


            server.Start();

            Console.WriteLine("Press space to stop");

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
