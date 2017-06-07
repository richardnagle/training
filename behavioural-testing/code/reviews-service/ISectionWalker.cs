using System.Collections.Generic;
using reviews_service.infrastructure;

namespace reviews_service
{
    public interface ISectionWalker
    {
        string GetText(IEnumerable<ReviewSection> sections, string sectionName);
    }
}