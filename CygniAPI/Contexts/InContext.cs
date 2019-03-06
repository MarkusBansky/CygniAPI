using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        /// he type of this request.
        /// </summary>
        public readonly RequestType RequestType;

        /// <summary>
        /// The current request url.
        /// </summary>
        public readonly string Url;

        /// <summary>
        /// Represents the whole body of the request in a string format.
        /// </summary>
        public readonly string Content;

        /// <summary>
        /// Gets the content encoding that can be used with data sent with the request.
        /// </summary>
        public readonly Encoding ContentEncoding;

        /// <summary>
        /// Gets the MIME type of the body data included in the request.
        /// </summary>
        public readonly string ContentType;

        /// <summary>
        /// Gets the length of the body data included in the request.
        /// </summary>
        public readonly long ContentLength;

        /// <summary>
        /// Gets the collection of header name/value pairs sent in the request.
        /// </summary>
        public readonly InHeader[] Headers;

        /// <summary>
        /// Gets a Boolean value that indicates whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol.
        /// </summary>
        public readonly bool IsSecureConnection;

        /// <summary>
        /// Gets a Boolean value that indicates whether the client sending this request is authenticated.
        /// </summary>
        public readonly bool IsUserAuthenticated;

        /// <summary>
        /// Gets the DNS name and, if provided, the port number specified by the client.
        /// </summary>
        public readonly string UserHostAddress;

        /// <summary>
        /// Gets the natural languages that are preferred for the response.
        /// </summary>
        public readonly string[] UserLanguages;

        /// <summary>
        /// Gets the DNS name and, if provided, the port number specified by the client.
        /// </summary>
        public readonly string UserHostName;

        /// <summary>
        /// Gets the user agent presented by the client.
        /// </summary>
        public readonly string UserAgent;

        /// <summary>
        /// An array that contains cookies that accompany the request. This property returns an empty collection if the request does not contain cookies.
        /// </summary>
        public readonly Cookie[] Cookies;

        internal InContext(HttpListenerRequest request, RequestType reqType, string url)
        {
            Headers = new InHeader[0];
            Cookies = new Cookie[0];

            Url = url;
            RequestType = reqType;

            // Create a reader and read all input data to the end
            // then set it to BodyText
            using var reader = new StreamReader(request.InputStream);
            Content = reader.ReadToEnd();

            ContentEncoding = request.ContentEncoding;
            ContentType = request.ContentType;
            ContentLength = request.ContentLength64;

            IsUserAuthenticated = request.IsAuthenticated;
            IsSecureConnection = request.IsSecureConnection;

            UserHostAddress = request.UserHostAddress;
            UserLanguages = request.UserLanguages;
            UserHostName = request.UserHostName;
            UserAgent = request.UserAgent;

            // Collect all headers from the input request
            Headers = request.Headers.AllKeys
                .Select(header => new InHeader { Key = header, Values = request.Headers.GetValues(header) })
                .ToArray();

            // Collect all Cookies from the input request
            Cookies = request.Cookies.ToArray();
        }
    }
}
