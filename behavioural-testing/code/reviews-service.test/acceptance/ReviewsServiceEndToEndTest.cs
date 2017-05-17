using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.acceptance
{
    [TestFixture]
    public class ReviewsServiceEndToEndTest
    {
        private readonly ServiceRunner _service = new ServiceRunner();
        private readonly Database _database = new Database();

        [SetUp]
        public void SetUp()
        {
            _database.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            _service.Stop();
        }

        [Test]
        public void Save_a_valid_review()
        {
            _service.ReceiveReview("valid.http");
            _database.AssertWasSaved(
                new ReviewDto
                {
                    ISBN = 9788175257665,
                    Reviewer = "Karen Castle",
                    Text = "<h1>A Classic of our Times</h1><h2>Karen Castle reviews Tolstoy's latest work</h2><p>Blah blah blah</p>",
                    Uri = "https://literaryreview.co.uk/"
                });
            _service.AssertHttpStatusCodeIs(201);
            _service.AssertOutputMessageIs("Review created");
        }

        [Test]
        public void Rejects_a_review_with_incorrect_content_type_header()
        {
            _service.ReceiveReview("bad_content_type.http");
            _database.AssertWasNotSaved();
            _service.AssertHttpStatusCodeIs(415);
            _service.AssertOutputMessageIs("Incorrect content type");
        }

        [Test]
        public void Rejects_a_review_with_incorrect_referer_header()
        {
            _service.ReceiveReview("bad_referer.http");
            _database.AssertWasNotSaved();
            _service.AssertHttpStatusCodeIs(400);
            _service.AssertOutputMessageIs("Bad referer uri");
        }

    }
}