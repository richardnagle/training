using reviews_service.infrastructure;

namespace reviews_service
{
    public class FormattedReview
    {
        public FormattedReview(long isbn, string reviewer, string uri, string text)
        {
            ISBN = isbn;
            Reviewer = reviewer;
            Uri = uri;
            Text = text;
        }

        public static FormattedReview FromPosted(PostedReview postedReview, Headers headers)
        {
            long isbn;
            isbn = long.TryParse(postedReview.ISBN, out isbn) ? isbn : 0;

            var textAsHtml = new HtmlText(postedReview.Sections).ToString();

            return new FormattedReview(
                isbn,
                postedReview.Reviewer,
                headers.Referer,
                textAsHtml);
        }

        public long ISBN { get; }
        public string Reviewer { get; }
        public string Uri { get; }
        public string Text { get; set; }
    }
}