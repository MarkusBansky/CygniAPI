using CygniAPI.Server;

namespace CygniAPI
{
    /// <summary>
    /// Contains all required configuration for the server.
    /// </summary>
    public class CygniConfiguration
    {
        /// <summary>
        /// Edit this field if you wish to add more request types supported by the server,
        /// or wish to remove any esxisting.
        /// </summary>
        public RequestType[] AllowedRequestTypes = new[] { RequestType.GET, RequestType.POST };

        /// <summary>
        /// Edit this field if you want server to be listening on different port.
        /// </summary>
        public int ListeningPort = 5050;

        /// <summary>
        /// Edit this field if you want to change the hostname of server listening endpoint.
        /// </summary>
        public string ListeningPath = "";
    }
}