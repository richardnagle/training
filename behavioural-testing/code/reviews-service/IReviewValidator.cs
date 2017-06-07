using reviews_service.infrastructure;

namespace reviews_service
{
    public interface IReviewValidator
    {
        bool ValidateContentType(Request<PostedReview> request);
        bool ValidateReferer(Request<PostedReview> request);
        bool ValidateISBN(Request<PostedReview> request);
    }
}