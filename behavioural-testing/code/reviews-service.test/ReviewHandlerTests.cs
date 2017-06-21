using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class ReviewHandlerTests
    {
        [Test]
        public void Valid_review_returns_http_201_with_no_error_message()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var handler = new ReviewHandler();
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }
    }
}