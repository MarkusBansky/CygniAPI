using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using static CygniAPI.Requests.Functions;

namespace CygniAPI.Server
{
    internal class BasicListener : IDisposable
    {
        private Thread       _thread;
        private HttpListener _listener;
        private bool         _isListening;

        private List<Callback> _registeredCallbacks;
        private CygniConfiguration _config;

        public BasicListener(CygniConfiguration config)
        {
            Initialize(config);
        }

        private void Initialize(CygniConfiguration config)
        {
            _config = config;
            _registeredCallbacks = new List<Callback>();
        }

        public void Start()
        {
            if(_isListening)
            {
                throw new Exception("The server is already running and listening.");
            }

            _thread = new Thread(Listen);
            _thread.Start();
        }

        public void Stop()
        {
            _isListening = false;

            _thread.Abort();
            _listener.Stop();
        }

        public void Dispose()
        {
            Stop();
        }

        private void Listen()
        {
            // Initialize the listener instance and set
            // all required parameters. Start listener.
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://{_config.ListeningPath}:{_config.ListeningPort.ToString()}/");
            _listener.Start();

            // Set the listening variable to true when
            // the service has started
            _isListening = true;

            // While loop to listen for the requests.
            while (_isListening)
            {
                try
                {
                    var inContext = _listener.GetContext();
                    Process(inContext);
                }
                catch
                {
                    _isListening = false;
                    Stop();
                }
            }
        }

        private void Process(HttpListenerContext c)
        {
            var registeredCallback = _registeredCallbacks
                .Where(rc => rc.RequestType == 
                    (RequestType)Enum.Parse(typeof(RequestType), c.Request.HttpMethod.ToUpperInvariant()))
                .FirstOrDefault();

            if(registeredCallback.RequestDelegate != null)
            {
                try
                {
                    // Create a reader and read all input data to the end
                    using var reader = new StreamReader(c.Request.InputStream);
                    var inputText = reader.ReadToEnd();

                    // Create input parameters for the request delegate that is defined by user
                    var builder = new StringBuilder();

                    // Invoke the method that is defined by the user
                    registeredCallback.RequestDelegate.Invoke(inputText, builder);
                    var responseText = builder.ToString();

                    // Add content information
                    c.Response.ContentType = "application/json";
                    c.Response.ContentLength64 = responseText.Length;

                    // Add headers
                    c.Response.AddHeader("Date", DateTime.UtcNow.ToString("r"));

                    // Write the response stream
                    var responseInBytes = Encoding.UTF8.GetBytes(responseText);
                    using var ms = new MemoryStream(responseInBytes);
                    var buffer = new byte[1024 * 16];
                    int nbytes;
                    // Write the bytes
                    while((nbytes = ms.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        c.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    ms.Close();

                    // Send closing response params
                    c.Response.StatusCode = (int)HttpStatusCode.OK;
                    c.Response.OutputStream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }

            // Close the response stream
            c.Response.OutputStream.Close();
        }

        #region Registering callbacks

        public void Get(string path, HostRequest reqFunction)
        {
            var cb = new Callback(path, reqFunction, RequestType.GET);
            _registeredCallbacks.Add(cb);
        }

        #endregion
    }
}
