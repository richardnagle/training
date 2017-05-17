using System.Collections.Generic;
using System.Linq;
using System.Text;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class HtmlText
    {
        private readonly IDictionary<string, string> _sections;

        private static readonly IEnumerable<SectionToTag> _sectionToTagMap =
            new List<SectionToTag>
            {
                new SectionToTag("Title", "h1"),
                new SectionToTag("SubTitle", "h2"),
                new SectionToTag("Body", "p")
            };

        public HtmlText(IEnumerable<ReviewSection> sections)
        {
            _sections = sections != null
                ? sections.ToDictionary(key => key.Name, val => val.Text)
                : new Dictionary<string, string>();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var sectionToTag in _sectionToTagMap)
            {
                AppendFormattedHtml(sectionToTag, result);
            }

            return result.ToString();
        }

        private void AppendFormattedHtml(SectionToTag sectionToTag, StringBuilder target)
        {
            var name = sectionToTag.SectionName;

            if (!_sections.ContainsKey(name))
            {
                return;
            }

            var innerText = _sections[name];
            var tag = sectionToTag.Tag;

            target.Append($"<{tag}>{innerText}</{tag}>");
        }

        private struct SectionToTag
        {
            public string SectionName { get; }
            public string Tag { get; }

            public SectionToTag(string sectionName, string tag)
            {
                SectionName = sectionName;
                Tag = tag;
            }
        }
    }
}