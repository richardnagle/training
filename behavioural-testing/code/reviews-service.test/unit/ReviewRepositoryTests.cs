using NSubstitute;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.unit
{
    [TestFixture]
    public class ReviewRepositoryTests
    {
        [Test]
        public void Saves_the_formatted_review_to_the_database_when_notified()
        {
            var database = Substitute.For<ISaveReviews>();
            var repo = new ReviewRepository(database, Substitute.For<IObserveSaving>());
            var formattedReview = new FormattedReview(1234, "the-reviewer", "the-uri", "the-text");

            repo.ReviewReadyForSaving(formattedReview);

            database.Received().Insert(
                Arg.Is<ReviewDto>(dto =>
                    dto.ISBN == 1234 &&
                    dto.Reviewer == "the-reviewer" &&
                    dto.Uri == "the-uri" &&
                    dto.Text == "the-text"));
        }

        [Test]
        public void Notifies_when_the_saving_request_is_complete()
        {
            var requestObserver = Substitute.For<IObserveSaving>();

            var repo = new ReviewRepository(Substitute.For<ISaveReviews>(), requestObserver);
            var formattedReview = new FormattedReview(1, "", "", "");

            repo.ReviewReadyForSaving(formattedReview);

            requestObserver.Received().ReviewSaved();
        }
    }
}