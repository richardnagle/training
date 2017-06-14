using NSubstitute;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class ReviewHtmlFormatterTests
    {
        [Test]
        public void Formats_text_to_html()
        {
            var reviewHtmlFormatter = new ReviewHtmlFormatter(Substitute.For<ISectionWalker>());
            var formatted = reviewHtmlFormatter.Format("the title", "the sub title", "the content");
            Assert.That(formatted, Is.EqualTo("<h1>the title</h1>\r\n<h2>the sub title</h2>\r\n<p>the content</p>\r\n"));
        }

        [Test]
        public void Formats_request_to_html()
        {
            var sections = new ReviewSection[0];
            var request = new PostedReview {Sections = sections};

            var sectionWalker = Substitute.For<ISectionWalker>();
            sectionWalker.GetText(sections, "Title").Returns("the title");
            sectionWalker.GetText(sections, "SubTitle").Returns("the sub title");
            sectionWalker.GetText(sections, "Body").Returns("the content");

            var reviewHtmlFormatter = new ReviewHtmlFormatter(sectionWalker);
            var formatted = reviewHtmlFormatter.Format(request);

            Assert.That(formatted, Is.EqualTo("<h1>the title</h1>\r\n<h2>the sub title</h2>\r\n<p>the content</p>\r\n"));
        }
    }
}