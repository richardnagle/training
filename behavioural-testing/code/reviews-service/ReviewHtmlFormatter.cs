using System.Text;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHtmlFormatter : IReviewHtmlFormatter
    {
        private readonly ISectionWalker _sectionWalker;

        public ReviewHtmlFormatter(ISectionWalker sectionWalker)
        {
            _sectionWalker = sectionWalker;
        }

        public string Format(PostedReview response)
        {
            var sections = response.Sections;

            return Format(
                _sectionWalker.GetText(sections, "Title"),
                _sectionWalker.GetText(sections, "SubTitle"),
                _sectionWalker.GetText(sections, "Body")
            );
        }

        public string Format(string title, string subTitle, string content)
        {
            var html = new StringBuilder();
            html.AppendLine($"<h1>{title}</h1>");
            html.AppendLine($"<h2>{subTitle}</h2>");
            html.AppendLine($"<p>{content}</p>");

            return html.ToString();
        }
    }
}