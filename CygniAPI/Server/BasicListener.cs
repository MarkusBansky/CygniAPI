using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using static CygniAPI.Requests.Functions;

namespace CygniAPI.Server
{
    internal class BasicListener : IDisposable
    {
        private Thread       _thread;
        private HttpListener _listener;
        private bool         _isListening;

        private List<HostRequest> _registeredCallbacks;

        public string ListenPath { get; private set; }

        public int ListenPort { get; private set; }

        public BasicListener(string path, int port)
        {
            Initialize(path, port);
        }

        private void Initialize(string path, int port)
        {
            _registeredCallbacks = new List<HostRequest>();

            ListenPath = path;
            ListenPort = port;

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
            _listener.Prefixes.Add($"http://*:{ListenPort.ToString()}/");
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
                }
                catch
                {
                    _isListening = false;
                    Stop();
                }
            }
        }

        #region Registering callbacks

        public void Get(string path, HostRequest reqFunction)
        {
            _registeredCallbacks.Add(reqFunction);
        }

        #endregion
    }
}
