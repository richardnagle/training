using System.Collections.Generic;
using System.Linq;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class Section
    {
        private readonly string _tag;
        private readonly string _text;

        private Section(ReviewSection reviewSection, string tag)
        {
            _text = reviewSection.Text;
            _tag = tag;
        }

        public override string ToString()
        {
            return $"<{_tag}>{_text}</{_tag}>";
        }

        public static implicit operator string(Section section)
        {
            return section.ToString();
        }

        public static Section Title(IEnumerable<ReviewSection> sections)
        {
            return new Section(GetSection("title", sections), "h1");
        }

        public static Section SubTitle(IEnumerable<ReviewSection> sections)
        {
            return new Section(GetSection("subtitle", sections), "h2");
        }

        public static Section Body(IEnumerable<ReviewSection> sections)
        {
            return new Section(GetSection("body", sections), "p");
        }

        private static ReviewSection GetSection(string sectionName, IEnumerable<ReviewSection> reviewSections)
        {
            return reviewSections.First(sect => sect.Name == sectionName);
        }
    }
}