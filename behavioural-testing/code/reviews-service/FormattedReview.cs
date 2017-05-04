using System.Linq;
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

            var sections = postedReview.Sections?.ToDictionary(
                key => key.Name,
                val => val.Text);

            var htmlText = sections != null
                ? $"<h1>{sections["Title"]}</h1><h2>{sections["SubTitle"]}</h2><p>{sections["Body"]}</p>"
                : string.Empty;

            return new FormattedReview(
                long.TryParse(postedReview.ISBN, out isbn) ? isbn : 0,
                postedReview.Reviewer,
                headers.Referer,
                htmlText);
        }

        public long ISBN { get; }
        public string Reviewer { get; }
        public string Uri { get; }
        public string Text { get; set; }
    }
}