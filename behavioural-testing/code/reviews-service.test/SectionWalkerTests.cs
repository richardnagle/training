using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class SectionWalkerTests
    {
        [TestCase("section1", ExpectedResult = "text for section 1")]
        [TestCase("section2", ExpectedResult = "section 2 text")]
        [TestCase("not-a-section", ExpectedResult = "")]
        public string Retrieves_the_text_for_a_given_named_section(string sectionName)
        {
            var sections = new[]
            {
                new ReviewSection {Name = "section1", Text = "text for section 1"},
                new ReviewSection {Name = "section2", Text = "section 2 text"}
            };

            return new SectionWalker().GetText(sections, sectionName);
        }
    }
}