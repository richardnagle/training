using reviews_service.infrastructure;

namespace reviews_service
{
    public class FormattedReview
    {
        public FormattedReview(long isbn, string reviewer, string uri)
        {
            ISBN = isbn;
            Reviewer = reviewer;
            Uri = uri;
        }

        public static FormattedReview FromPosted(PostedReview postedReview, Headers headers)
        {
            long isbn;

            return new FormattedReview(
                long.TryParse(postedReview.ISBN, out isbn) ? isbn : 0,
                postedReview.Reviewer,
                headers.Referer);
        }

        public long ISBN { get; }
        public string Reviewer { get; }
        public string Uri { get; }
    }
}