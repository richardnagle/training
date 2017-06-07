using System.Collections.Generic;
using System.Linq;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class SectionWalker
    {
        public string GetText(IEnumerable<ReviewSection> sections, string sectionName)
        {
            var namedSection = sections.FirstOrDefault(x => x.Name == sectionName) ?? new ReviewSection { Text = ""};
            return namedSection.Text;
        }
    }
}