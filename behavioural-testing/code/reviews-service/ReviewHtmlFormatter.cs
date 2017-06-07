using System.Text;

namespace reviews_service
{
    public class ReviewHtmlFormatter
    {
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