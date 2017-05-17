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

            var htmlText = string.Empty;

            if (postedReview.Sections != null)
            {
                var sections = postedReview.Sections.ToDictionary(
                    key => key.Name,
                    val => val.Text);

                var title = sections.ContainsKey("Title") ? $"<h1>{sections["Title"]}</h1>" : string.Empty;
                var subTitle = sections.ContainsKey("SubTitle") ? $"<h2>{sections["SubTitle"]}</h2>" : string.Empty;
                var body = sections.ContainsKey("Body") ? $"<p>{sections["Body"]}</p>" : string.Empty;

                htmlText = string.Concat(title, subTitle, body);
            }

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