using System.Net;

namespace CygniAPI.Contexts
{
    /// <summary>
    /// Holds the context information that is transfered from the user url
    /// to the function that handles the API call.
    /// </summary>
    public class InContext
    {
        /// <summary>
        /// Holds the main object that is passed from the Http server
        /// </summary>
        public readonly HttpListenerContext Context;

        /// <summary>
        /// Initializes a context that is passed into the function that handles the API call.
        /// This class handles the input data to the function from the user request.
        /// </summary>
        /// <param name="context">A raw context from Http server.</param>
        public InContext(HttpListenerContext context)
        {
            Context = context;
        }
    }
}
