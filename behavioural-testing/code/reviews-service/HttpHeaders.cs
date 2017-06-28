using System.Collections.Generic;

namespace reviews_service
{
    public class HttpHeaders
    {
        private readonly IDictionary<string, string> _requestHeaders;

        public HttpHeaders(IDictionary<string, string> requestHeaders)
        {
            _requestHeaders = requestHeaders;
        }

        public ContentType ContentType => new ContentType(_requestHeaders["Content-type"]);

        public Referer Referer => new Referer(_requestHeaders["Referer"]);
    }
}