using System.Text.RegularExpressions;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class Referer
    {
        private readonly string _uri;

        public Referer(string uri)
        {
            _uri = uri;
        }

        public bool IsInvalid()
        {
            return !Regex.IsMatch(_uri, "http(s)?://(.*).(.*)");
        }

        public Response GetErrorResponse()
        {
            return new Response(400, "Bad referer uri");
        }
    }
}