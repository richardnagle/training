using System.Collections.Generic;

namespace reviews_service
{
    public class Headers
    {
        private readonly IDictionary<string, string> _httpHeaders;

        public Headers(IDictionary<string, string> httpHeaders)
        {
            _httpHeaders = httpHeaders;
        }

        public string Referer => GetValueOrEmpty("Referer");

        private string GetValueOrEmpty(string headerName)
        {
            string value;
            return _httpHeaders.TryGetValue(headerName, out value)
                ? value
                : string.Empty;
        }
    }
}