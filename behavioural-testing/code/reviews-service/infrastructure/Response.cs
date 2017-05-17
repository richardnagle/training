namespace reviews_service.infrastructure
{
    public class Response
    {
        public Response(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get;}
        public string Message { get; }
    }
}
