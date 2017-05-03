using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using reviews_service.infrastructure;

namespace reviews_service
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("** Start **");

            var header = args[0];
            var body = args[1];

            var request = new RequestSerializer().Deserialize(header, body);
            var handler = new ReviewHandler(new RequestRepository()).Handle(request);

            Console.WriteLine("** End **");
        }

        private class RequestSerializer
        {
            public Request<PostedReview> Deserialize(string header, string body)
            {
                return new Request<PostedReview>(
                    DeserializeBody(body),
                    DeserializeHeaders(header));
            }

            private IDictionary<string, string> DeserializeHeaders(string header)
            {
                var headers = header.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                return headers
                    .Select(h => h.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(key => key[0], value => value[1]);
            }

            private PostedReview DeserializeBody(string body)
            {
                return JsonConvert.DeserializeObject<PostedReview>(body);
            }
        }
    }
}