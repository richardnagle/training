using System;
using System.IO;

namespace reviews_service.test.acceptance
{
    public class ServiceRunner
    {
        private ExternalProgram _serviceProcess;

        public void Stop()
        {
            if (_serviceProcess == null) return;

            _serviceProcess.Dispose();
            _serviceProcess = null;
        }

        public void ReceiveReview(string exampleRequestName)
        {
            var request = UseRequest(exampleRequestName);
            StartService(request);
        }

        private Request UseRequest(string exampleRequestName)
        {
            var assembly = GetType().Assembly;
            var resourceName = $"{GetType().Namespace}.requests.{exampleRequestName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var request = reader.ReadToEnd();
                var requestParts = request.Split(new[] {"---\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                return new Request(requestParts[0], requestParts[1]);
            }
        }

        private void StartService(Request request)
        {
            const string processName =
                @"c:\_root\training\behavioural-testing\code\reviews-service\bin\debug\reviews-service.exe";

            Stop();

            _serviceProcess = new ExternalProgram(processName, request.Header, request.Body);
            _serviceProcess.Start();
            _serviceProcess.WaitForExit(TimeSpan.FromSeconds(5));
        }

        private class Request
        {
            public string Header { get; }
            public string Body { get; }

            public Request(string header, string body)
            {
                Header = header;
                Body = body;
            }
        }
    }
}