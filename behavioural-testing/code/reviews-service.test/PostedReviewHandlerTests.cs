using NUnit.Framework;

namespace reviews_service.test
{
    [TestFixture]
    public class PostedReviewHandlerTests
    {
        [Test]
        public void When_content_type_is_not_json_returns_415_with_error_message()
        {
            var request = new PostedReviewBuilder()
                .WithContentType("**invalid content type **")
                .Build();

            var handler = new PostedReviewHandler();

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(415));
            Assert.That(response.Error, Is.EqualTo("Incorrect content type"));
        }
    }
}