using System;
using System.IO;
using System.Net;
using CygniAPI;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private static CygniApiServer Server;

        [SetUp]
        public void Setup()
        {
            Server = new CygniApiServer(new CygniAPI.Server.CygniConfiguration { ListeningPath = "localhost", ListeningPort = 8080 });
        }

        [Test]
        public void RegisterTheCallbackForSimpleGet()
        {
            Server.Get("/test", (i, o) =>
            {
                Console.WriteLine(i);
                o.Append("THIS IS JUST A TEST");
            });
        }

        [Test]
        public void StartTheServer()
        {
            RegisterTheCallbackForSimpleGet();

            Server.Start();
        }

        [Test]
        public void MakeATestReguest()
        {
            RegisterTheCallbackForSimpleGet();
            StartTheServer();

            Requests.Get("http://localhost:8080/test");
        }
    }

    static class Requests
    {
        public static string Get(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}