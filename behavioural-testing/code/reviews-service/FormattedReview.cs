using System.Collections.Generic;
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

                var title = FormatHtml(sections, "Title", "h1");
                var subTitle = FormatHtml(sections, "SubTitle", "h2");
                var body = FormatHtml(sections, "Body", "p");

                htmlText = string.Concat(title, subTitle, body);
            }

            return new FormattedReview(
                long.TryParse(postedReview.ISBN, out isbn) ? isbn : 0,
                postedReview.Reviewer,
                headers.Referer,
                htmlText);
        }

        private static string FormatHtml(IDictionary<string, string> sections, string name, string tag)
        {
            return sections.ContainsKey(name) ? $"<{tag}>{sections[name]}</{tag}>" : string.Empty;
        }

        public long ISBN { get; }
        public string Reviewer { get; }
        public string Uri { get; }
        public string Text { get; set; }
    }
}