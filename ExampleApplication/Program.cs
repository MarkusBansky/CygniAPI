using System;

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
            var server = new CygniAPI.CygniApiServer(new CygniAPI.Server.CygniConfiguration
            {
                ListeningPath = "localhost",
                ListeningPort = 8080
            });
            server.Get("test", (i, o) =>
            {
                Console.WriteLine("THIS IS FUKING TEST");
            });
            server.Start();

            Console.WriteLine("Press space to stop");

            var result = 0;
            while(result != (int)ConsoleKey.Spacebar)
            {
                result = (int)Console.ReadKey().Key;
            }
        }
    }
}
