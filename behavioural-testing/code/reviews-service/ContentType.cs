using reviews_service.infrastructure;

namespace reviews_service
{
    public class ContentType: IValidateAReview
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

        public Response GetErrorResponse()
        {
            return new Response(415, "Incorrect content type");
        }
    }
}