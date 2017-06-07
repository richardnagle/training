using System.Collections.Generic;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class ReviewDtoMapperTests
    {
        [Test]
        public void Maps_referer_from_http_header_to_review_dto()
        {
            var headers = new Dictionary<string, string>
            {
                ["Referer"] = "the referer"
            };

            var reviewDto = new ReviewDto();

            new ReviewDtoMapper().MapHttpHeaders(headers, reviewDto);

            Assert.That(reviewDto.Uri, Is.EqualTo("the referer"));
        }

        [Test]
        public void Maps_isbn_to_review_dto()
        {
            var postedReview = new PostedReview
            {
                ISBN = "1234567890123"
            };

            var reviewDto = new ReviewDto();

            new ReviewDtoMapper().MapBodyFields(postedReview, reviewDto);

            Assert.That(reviewDto.ISBN, Is.EqualTo(1234567890123));
        }

        [Test]
        public void Maps_reviewer_name_to_review_dto()
        {
            var postedReview = new PostedReview
            {
                Reviewer = "the reviewer",
                ISBN = "0"
            };

            var reviewDto = new ReviewDto();

            new ReviewDtoMapper().MapBodyFields(postedReview, reviewDto);

            Assert.That(reviewDto.Reviewer, Is.EqualTo("the reviewer"));
        }

        [Test]
        public void Maps_text_to_review_dto()
        {
            var reviewDto = new ReviewDto();

            new ReviewDtoMapper().MapText("the text", reviewDto);

            Assert.That(reviewDto.Text, Is.EqualTo("the text"));
        }
    }
}