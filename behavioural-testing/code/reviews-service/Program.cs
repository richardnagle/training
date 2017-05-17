using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using reviews_service.infrastructure;

namespace reviews_service
{
    public static class Program
    {
        public static int Main(params string[] args)
        {
            Console.WriteLine("** Start **");

            var header = args[0];
            var body = args[1];

            var processor = new RequestProcessor();
            var response = processor.Start(header, body);

            Console.WriteLine(response.Message);

            return response.StatusCode;
        }

        private class RequestProcessor: IObserveSaving
        {
            private readonly ManualResetEventSlim _processingHasFinished;
            private Response _response;

            public RequestProcessor()
            {
                _processingHasFinished = new ManualResetEventSlim(false);
            }

            public Response Start(string headers, string body)
            {
                var request = new RequestSerializer().Deserialize(headers, body);
                var savingObserver = new SavingObserver();
                var handler = new ReviewHandler(new ReviewRepository(new DatabaseWriter(), savingObserver), savingObserver);

                savingObserver.Subscribe(this);
                savingObserver.Subscribe(handler);

                Task.Run(() => handler.Handle(request));

                _processingHasFinished.Wait(TimeSpan.FromSeconds(20));

                return _response;
            }

            public void ReviewSaved()
            {
                Complete(201, "Review created");
            }

            public void ReviewNotSaved(int httpStatusCode, string errorMessage)
            {
                Complete(httpStatusCode, errorMessage);
            }

            private void Complete(int httpStatusCode, string errorMessage)
            {
                _response = new Response(httpStatusCode, errorMessage);
                _processingHasFinished.Set();
            }
        }

        private class SavingObserver : IObserveSaving
        {
            private readonly List<IObserveSaving> _observers = new List<IObserveSaving>();

            public void Subscribe(IObserveSaving observer)
            {
                _observers.Add(observer);
            }

            public void ReviewSaved()
            {
                foreach (var observer in _observers)
                {
                    observer.ReviewSaved();
                }
            }

            public void ReviewNotSaved(int httpStatusCode, string errorMessage)
            {
                foreach (var observer in _observers)
                {
                    observer.ReviewNotSaved(httpStatusCode, errorMessage);
                }
            }
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
                    .ToDictionary(key => key[0], value => string.Join(":", value.Skip(1)).Trim());
            }

            private PostedReview DeserializeBody(string body)
            {
                return JsonConvert.DeserializeObject<PostedReview>(body);
            }
        }
    }
}