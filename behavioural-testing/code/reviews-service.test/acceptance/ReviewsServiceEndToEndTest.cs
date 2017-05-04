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
        public void Save_an_empty_review()
        {
            _service.ReceiveReview("empty.http");
            _database.AssertWasSaved(new ReviewDto());
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
        }
    }
}