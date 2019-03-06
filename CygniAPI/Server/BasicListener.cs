using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CygniAPI.Contexts;
using static CygniAPI.Requests.Functions;

namespace CygniAPI.Server
{
    internal class BasicListener : IDisposable
    {
        #region Private Variables
        private CancellationTokenSource _threadCTS;
        private HttpListener _listener;
        private bool         _isListening;
        private bool         _isDisposed;

        private List<Callback> _registeredCallbacks;
        private CygniConfiguration _config;
        #endregion

        #region Constructors
        public BasicListener(CygniConfiguration config)
        {
            Initialize(config);
        }
        #endregion

        #region Basic Server Operations (Start, Stop, Dispose, Initialize)
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

            _threadCTS = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(new WaitCallback(Listen), _threadCTS.Token);
        }

        public void Stop()
        {
            _isListening = false;

            _threadCTS.Cancel();
            _listener.Stop();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            Stop();
            _threadCTS.Dispose();

            _isDisposed = true;
        }
        #endregion

        #region Server Listening Thread Method
        private void Listen(object state)
        {
            // Make a cancelation token
            var token = (CancellationToken)state;

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
                if (token.IsCancellationRequested) break;

                try
                {
                    var inContext = _listener.GetContext();
                    var request = inContext.Request;

                    // Sort out the call to the favicon
                    var url = request.RawUrl.Substring(1);
                    if(url == "favicon.ico") continue;

                    // Process the call
                    Process(inContext);
                }
                catch
                {
                    _isListening = false;
                    Stop();
                }
            }
        }
        #endregion

        private void Process(HttpListenerContext c)
        {
            var registeredCallback = _registeredCallbacks
                .Where(rc => 
                    rc.Url == c.Request.RawUrl && 
                    rc.RequestType == (RequestType)Enum.Parse(typeof(RequestType), c.Request.HttpMethod.ToUpperInvariant()))
                .FirstOrDefault();

            if(registeredCallback.RequestDelegate != null)
            {
                try
                {
                    // Create input parameters for the request delegate that is defined by user
                    var builder = new StringBuilder();
                    var inContext = new InContext(c.Request, registeredCallback.RequestType, registeredCallback.Url);

                    // Invoke the method that is defined by the user
                    registeredCallback.RequestDelegate.Invoke(inContext, builder);
                    var responseText = builder.ToString();

                    // Add content information
                    c.Response.ContentType = "application/json";

                    // Add headers
                    c.Response.AddHeader("Date", DateTime.UtcNow.ToString("r"));

                    // Write the response stream
                    var responseInBytes = Encoding.UTF8.GetBytes(responseText);
                    c.Response.ContentLength64 = responseInBytes.Length;

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
