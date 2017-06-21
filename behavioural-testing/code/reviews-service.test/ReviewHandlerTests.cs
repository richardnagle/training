using System.Collections.Generic;
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

            var request = new Request<PostedReview>(
                new PostedReview
                {
                    ISBN = "1234567890123",
                    Sections = new ReviewSection[0],
                },
                new Dictionary<string, string>
                {
                    ["Referer"] = "http://the-referer.com"
                });

            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [Test]
        public void Valid_review_is_saved_in_database()
        {
            var database = Substitute.For<ISaveReviews>();

            var handler = new ReviewHandler(new ReviewDtoMapper(), new SectionWalker(), new ReviewHtmlFormatter(), database);
            var request = new Request<PostedReview>(
                new PostedReview
                {
                    ISBN = "1234567890123",
                    Sections = new[]
                    {
                        new ReviewSection { Name = "title", Text = "the-title"},
                        new ReviewSection { Name = "subtitle", Text = "the-sub-title"},
                        new ReviewSection { Name = "body", Text = "the-body"},
                    }
                },
                new Dictionary<string, string>
                {
                    ["Referer"] = "http://the-referer.com"
                });

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