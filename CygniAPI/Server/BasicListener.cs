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
            _listener.Prefixes.Add($"http://*:{_config.ListeningPort.ToString()}/");
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
            var cb = new Callback(path, reqFunction, RequestType.GET);
            _registeredCallbacks.Add(cb);
        }

        #endregion
    }
}
