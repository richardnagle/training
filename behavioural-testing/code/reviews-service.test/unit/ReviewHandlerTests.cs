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
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_isbn()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                ISBN = "42"
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.ISBN == 42));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_reviewer()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Reviewer = "Paul",
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Reviewer == "Paul"));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_uri()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var headers = new Dictionary<string, string>
            {
                ["Referer"] = "http://company.com"
            };

            var request = new Request<PostedReview>(new PostedReview(), headers);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Uri == "http://company.com"));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_all_sections_formatted_as_html()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Sections = new[]
                {
                    new ReviewSection { Name = "Title", Text = "the title" },
                    new ReviewSection { Name = "SubTitle", Text = "the sub title" },
                    new ReviewSection { Name = "Body", Text = "the body" }
                }
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Text ==
                    "<h1>the title</h1><h2>the sub title</h2><p>the body</p>"));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_title_formatted_as_html()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Sections = new[]
                {
                    new ReviewSection { Name = "Title", Text = "the title" },
                }
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Text == "<h1>the title</h1>"));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_sub_title_formatted_as_html()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Sections = new[]
                {
                    new ReviewSection { Name = "SubTitle", Text = "the sub title" },
                }
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Text == "<h2>the sub title</h2>"));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_body_formatted_as_html()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Sections = new[]
                {
                    new ReviewSection { Name = "Body", Text = "the body" }
                }
            };

            var request = new Request<PostedReview>(review);

            var handler = new ReviewHandler(reviewObserver);
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Text == "<p>the body</p>"));
        }
    }
}