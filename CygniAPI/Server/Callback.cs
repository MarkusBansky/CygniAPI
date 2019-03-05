using static CygniAPI.Requests.Functions;

namespace CygniAPI.Server
{
    internal struct Callback
    {
        public string Url;

        public HostRequest RequestDelegate;
        public RequestType RequestType;

        public Callback(string url, HostRequest req, RequestType type)
        {
            Url = url;
            RequestDelegate = req;
            RequestType = type;
        }
    }
}
