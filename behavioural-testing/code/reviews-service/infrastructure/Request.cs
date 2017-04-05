using System.Collections.Generic;

namespace reviews_service.infrastructure
{
    public class Request<T>
    {
        public Request(T body, IDictionary<string, string> headers = null)
        {
            Headers = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>();
            Body = body;
        }

        public IReadOnlyDictionary<string, string> Headers { get; }
        public T Body { get; }
    }
}
