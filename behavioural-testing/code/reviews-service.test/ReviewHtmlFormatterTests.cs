using NUnit.Framework;

namespace reviews_service.test
{
    [TestFixture]
    public class ReviewHtmlFormatterTests
    {
        [Test]
        public void Formats_text_to_html()
        {
            var formatted = new ReviewHtmlFormatter().Format("the title", "the sub title", "the content");
            Assert.That(formatted, Is.EqualTo("<h1>the title</h1>\r\n<h2>the sub title</h2>\r\n<p>the content</p>\r\n"));
        }
    }
}