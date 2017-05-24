using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        private string ContentType => GetValueOrEmpty("Content-type");

        private string GetValueOrEmpty(string headerName)
        {
            string value;
            return _httpHeaders.TryGetValue(headerName, out value)
                ? value
                : string.Empty;
        }

        public void Validate(IObserveValidation savingObserver)
        {
            if (ContentType != "application/json")
            {
                savingObserver.ReviewFailedValidation(415, "Incorrect content type");
            }

            if (!Regex.IsMatch(Referer, "http(s)?://(.*).(.*)"))
            {
                savingObserver.ReviewFailedValidation(400, "Bad referer uri");
            }
        }
    }
}