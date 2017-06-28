using reviews_service.infrastructure;

namespace reviews_service
{
    public interface IValidateAReview
    {
        bool IsInvalid();
        Response GetErrorResponse();
    }
}