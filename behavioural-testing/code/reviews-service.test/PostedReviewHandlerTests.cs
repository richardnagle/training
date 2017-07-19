using NSubstitute;
using NUnit.Framework;
using reviews_service.infrastructure;

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

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(415));
            Assert.That(response.Error, Is.EqualTo("Incorrect content type"));
        }

        [Test]
        public void When_content_type_is_valid_returns_201_with_no_error_message()
        {
            var request = new PostedReviewBuilder()
                .WithContentType("application/json")
                .Build();

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [TestCase("")]
        [TestCase("just some text")]
        public void When_referer_has_invalid_uri_returns_400_with_error_message(string referer)
        {
            var request = new PostedReviewBuilder()
                .WithRefererUrl(referer)
                .Build();

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Bad referer uri"));
        }

        [TestCase("http://somecompany.com")]
        [TestCase("https://somecompany.com")]
        public void When_referer_uri_is_valid_returns_201_with_no_error_message(string referer)
        {
            var request = new PostedReviewBuilder()
                .WithRefererUrl(referer)
                .Build();

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [TestCase("123456789012")]
        [TestCase("12345678901234")]
        [TestCase("XXXXXXXXXXXXXX")]
        public void When_referer_has_invalid_isbn_returns_400_with_error_message(string isbn)
        {
            var request = new PostedReviewBuilder()
                .WithIsbn(isbn)
                .Build();

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Invalid isbn"));
        }

        [Test]
        public void When_isbn_is_valid_returns_201_with_no_error_message()
        {
            var request = new PostedReviewBuilder()
                .WithIsbn("1234567890123")
                .Build();

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>());

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [Test]
        public void When_review_is_valid_it_is_saved()
        {
            var database = Substitute.For<ISaveReviews>();

            var request = new PostedReviewBuilder()
                .WithIsbn("1234567890123")
                .WithReviewer("Jane Bloggs")
                .WithRefererUrl("https://tempuri.org/review.html")
                .WithBodyTitle("Title")
                .WithBodySubTitle("SubTitle")
                .WithBodyText("Body")
                .Build();

            var handler = new PostedReviewHandler(database);

            handler.Handle(request);

            database.Received().Insert(Arg.Is<ReviewDto>(dto => dto.ISBN == 1234567890123));
            database.Received().Insert(Arg.Is<ReviewDto>(dto => dto.Reviewer == "Jane Bloggs"));
            database.Received().Insert(Arg.Is<ReviewDto>(dto => dto.Uri == "https://tempuri.org/review.html"));
            database.Received().Insert(Arg.Is<ReviewDto>(dto => dto.Text == "<h1>Title</h1>\r\n<h2>SubTitle</h2>\r\n<p>Body</p>"));
        }
    }
}