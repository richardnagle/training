using reviews_service.infrastructure;

namespace reviews_service
{
    public class ContentType
    {
        private readonly string _value;

        public ContentType(string value)
        {
            _value = value;
        }

        public bool IsInvalid()
        {
            return _value != "application/json";
        }

        public Response GetErrorReponse()
        {
            return new Response(415, "Incorrect content type");
        }
    }
}