using reviews_service.infrastructure;

namespace reviews_service
{
    public interface IReviewHtmlFormatter
    {
        string Format(string title, string subTitle, string content);
        string Format(PostedReview response);
    }
}