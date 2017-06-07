using NUnit.Framework;
using NSubstitute;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class PostedReviewHandlerTests
    {
        [Test]
        public void Responds_with_http_415_when_content_type_header_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(false);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator);
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(415));
            Assert.That(response.Error, Is.EqualTo("Incorrect content type"));
        }

        [Test]
        public void Responds_with_http_400_when_referer_uri_is_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(false);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator);
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Bad referer uri"));
        }

        [Test]
        public void Responds_with_http_400_when_isbn_is_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(false);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator);
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Invalid ISBN"));
        }

        [Test]
        public void Responds_with_http_201_when_request_is_valid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator);
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [Test]
        public void Saves_review_when_request_is_valid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var databaseService = Substitute.For<ISaveReviews>();

            var handler = new PostedReviewHandler(databaseService, validator);
            handler.Handle(request);

            databaseService.Received().Insert(Arg.Any<ReviewDto>());
        }
    }
}