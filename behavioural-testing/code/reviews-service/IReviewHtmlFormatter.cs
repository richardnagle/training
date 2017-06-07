namespace reviews_service
{
    public interface IReviewHtmlFormatter
    {
        string Format(string title, string subTitle, string content);
    }
}