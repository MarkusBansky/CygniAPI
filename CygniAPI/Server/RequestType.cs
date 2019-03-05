namespace CygniAPI.Server
{
    /// <summary>
    /// Types of http requests that are supported by the server.
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// A basic GET request used to retreive data from the server.
        /// </summary>
        GET,
        /// <summary>
        /// A basic POST request that sends some input data to the server. Can receive any data.
        /// </summary>
        POST,
        /// <summary>
        /// PUT request that only sends the data to the server.
        /// </summary>
        PUT,
        /// <summary>
        /// Makes a request to delete some data, does not send or receive any data in the body.
        /// </summary>
        DELETE
    }
}
