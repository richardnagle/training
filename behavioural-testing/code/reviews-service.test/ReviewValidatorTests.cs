using System.Collections.Generic;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class ReviewValidatorTests
    {
        [TestCase("application/json", ExpectedResult = true)]
        [TestCase("text/html", ExpectedResult = false)]
        public bool Validates_that_content_type_header_is_json(string contentType)
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-type"] = contentType
            };

            var request = new Request<PostedReview>(new PostedReview(), headers);

            var validator = new ReviewValidator();

            return validator.ValidateContentType(request);
        }

        [TestCase("http://somecompany.com", ExpectedResult = true)]
        [TestCase("https://somecompany.com", ExpectedResult = true)]
        [TestCase("", ExpectedResult = false)]
        [TestCase("just some text", ExpectedResult = false)]
        //etc...
        public bool Validates_that_referer_is_valid_uri(string referer)
        {
            var headers = new Dictionary<string, string>
            {
                ["Referer"] = referer
            };

            var request = new Request<PostedReview>(new PostedReview(), headers);

            var validator = new ReviewValidator();

            return validator.ValidateReferer(request);
        }

        [TestCase("1234567890123", ExpectedResult = true)]
        [TestCase("123456789012", ExpectedResult = false)]
        [TestCase("12345678901234", ExpectedResult = false)]
        [TestCase("XXXXXXXXXXXXXX", ExpectedResult = false)]
        public bool Validates_that_isbn_is_thirteen_digits(string isbn)
        {
            var request = new Request<PostedReview>(new PostedReview { ISBN = isbn });

            var validator = new ReviewValidator();

            return validator.ValidateISBN(request);
        }
    }
}
