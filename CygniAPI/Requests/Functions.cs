using CygniAPI.Contexts;

namespace CygniAPI.Requests
{
    /// <summary>
    /// Handles main delegate functions that are used in the API calls.
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// Used to handle the API call to a specific URL address, when the call is executed then this method is
        /// going to be invoked and then the main Http context that is an input is going to be passed into. After this function processes
        /// the output it is passed to the next parameter.
        /// </summary>
        /// <param name="cIn">The input context from Http server.</param>
        public delegate void HostRequest(InContext cIn);
    }
}
