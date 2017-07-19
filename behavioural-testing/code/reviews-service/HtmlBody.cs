using System.Collections.Generic;
using System.Text;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class HtmlBody: IPopulateReviewDto
    {
        private readonly IEnumerable<ReviewSection> _sections;

        public HtmlBody(IEnumerable<ReviewSection> sections)
        {
            _sections = sections;
        }

        public void Populate(ReviewDto reviewDto)
        {
            reviewDto.Text = FormatSectionAsHtml();
        }

        private string FormatSectionAsHtml()
        {
            var text = new StringBuilder();
            text.AppendLine(Section.Title(_sections));
            text.AppendLine(Section.SubTitle(_sections));
            text.Append(Section.Body(_sections));

            return text.ToString();
        }
    }
}