namespace reviews_service.infrastructure
{
    public class Response
    {
        public Response(int statusCode, string error)
        {
            StatusCode = statusCode;
            Error = error;
        }

        public int StatusCode { get;}
        public string Error { get; }
    }
}
