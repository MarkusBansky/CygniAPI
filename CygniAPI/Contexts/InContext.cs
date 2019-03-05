using System.Text;
using CygniAPI.Server;

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
        public readonly StringBuilder SBuilder;

        /// <summary>
        /// he type of this request.
        /// </summary>
        public readonly RequestType RequestType;

        /// <summary>
        /// Initializes a context that is passed into the function that handles the API call.
        /// This class handles the input data to the function from the user request.
        /// </summary>
        /// <param name="builder">A raw string builder from Http server.</param>
        /// <param name="reqType">The type of request that is held.</param>
        public InContext(StringBuilder builder, RequestType reqType)
        {
            SBuilder = builder;
            RequestType = reqType;
        }
    }
}
