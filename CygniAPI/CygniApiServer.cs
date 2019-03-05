using CygniAPI.Server;
using static CygniAPI.Requests.Functions;

namespace CygniAPI
{
    /// <summary>
    /// CygniAPI is a lightweight sowftware package that helps you easily create and host API requests and create a very basic RESTful service.
    /// </summary>
    public sealed class CygniApiServer
    {
        private BasicListener _listener;

        /// <summary>
        /// A basic configured API server.
        /// </summary>
        public CygniApiServer()
        {
            _listener = new BasicListener(new CygniConfiguration());
        }

        /// <summary>
        /// Constructor creates a new listener and applies the configuration.
        /// </summary>
        /// <param name="config">A basic server configuration.</param>
        public CygniApiServer(CygniConfiguration config)
        {
            _listener = new BasicListener(config);
        }

        /// <summary>
        /// Descructor used to dispose any objects from the memory.
        /// </summary>
        ~CygniApiServer()
        {
            _listener.Dispose();
        }

        /// <summary>
        /// Starts the server to listen on the selected port and host.
        /// </summary>
        public void Start()
        {
            _listener.Start();
        }

        /// <summary>
        /// Stops the server from running.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
        }

        /// <summary>
        /// Register a GET callback to the server with specified function that handles the input and performs the output.
        /// </summary>
        /// <param name="path">The path that the server has to be listening to.</param>
        /// <param name="reqFunction">The function that performs all the calculations.</param>
        public void Get(string path, HostRequest reqFunction)
        {
            _listener.Get(path, reqFunction);
        }
    }
}
