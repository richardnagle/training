using System.Text.RegularExpressions;
using NSubstitute;
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
            var handler = new ReviewHandler(new ReviewDtoMapper(), new SectionWalker(), new ReviewHtmlFormatter(), Substitute.For<ISaveReviews>());
            var request = new PostedReviewBuilder().Build();

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [Test]
        public void Valid_review_is_saved_in_database()
        {
            var database = Substitute.For<ISaveReviews>();

            var handler = new ReviewHandler(new ReviewDtoMapper(), new SectionWalker(), new ReviewHtmlFormatter(), database);
            var request = new PostedReviewBuilder()
                .WithIsbn("1234567890123")
                .WithBodyTitle("the-title")
                .WithBodySubTitle("the-sub-title")
                .WithBodyText("the-body")
                .WithRefererUrl("http://the-referer.com")
                .Build();

            var response = handler.Handle(request);

            database
                .Received()
                .Insert(Arg.Is<ReviewDto>(dto =>
                        dto.ISBN == 1234567890123 &&
                        Regex.IsMatch(dto.Text, "(?s).*the-title.*the-sub-title.*the-body.*") &&
                        dto.Uri == "http://the-referer.com"));
        }
    }
}