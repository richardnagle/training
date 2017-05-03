using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.unit
{
    [TestFixture]
    public class ReviewHandlerTests
    {
        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                ISBN = "42",
                Reviewer = "Paul",
            };

            var headers = new Dictionary<string, string>
            {
                ["Referer"] = "http://company.com"
            };

            var request = new Request<PostedReview>(review, headers);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver.Received().ReviewReadyForSaving(
                Arg.Is<FormattedReview>(revw =>
                    revw.ISBN == 42 &&
                    revw.Reviewer == "Paul" &&
                    revw.Uri == "http://company.com"));
        }
    }
}