using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.unit
{
    [TestFixture]
    public class ReviewHandlerTests
    {
        private readonly IDictionary<string, string> _validHeaders =
            new Dictionary<string, string>
            {
                ["Content-type"] = "application/json",
                ["Referer"] = "https://www.company.com/default.html"
            };

        private readonly PostedReview _validPostedReview =
            new PostedReview
            {
                ISBN = "1234567890123"
            };

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_isbn()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.ISBN == 1234567890123));
        }

        [Test]
        public void Notifies_when_the_review_is_formatted_ready_for_saving_with_reviewer()
        {
            var reviewObserver = Substitute.For<IObserveReview>();

            var review = new PostedReview
            {
                Reviewer = "Paul",
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
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
                ["Referer"] = "http://company.com",
                ["Content-type"] = "application/json"
            };

            var request = new Request<PostedReview>(_validPostedReview, headers);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
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
                    new ReviewSection {Name = "Title", Text = "the title"},
                    new ReviewSection {Name = "SubTitle", Text = "the sub title"},
                    new ReviewSection {Name = "Body", Text = "the body"}
                },
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
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
                    new ReviewSection {Name = "Title", Text = "the title"},
                },
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
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
                    new ReviewSection {Name = "SubTitle", Text = "the sub title"},
                },
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
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
                    new ReviewSection {Name = "Body", Text = "the body"}
                },
                ISBN = "1234567890123"
            };

            var request = new Request<PostedReview>(review, _validHeaders);

            var handler = new ReviewHandler(reviewObserver, Substitute.For<IObserveValidation>());
            handler.Handle(request);

            reviewObserver
                .Received()
                .ReviewReadyForSaving(Arg.Is<FormattedReview>(revw => revw.Text == "<p>the body</p>"));
        }

        [Test]
        public void Does_not_notify_for_save_when_validation_fails()
        {
            var handler = new ReviewHandler(Substitute.For<IObserveReview>(), Substitute.For<IObserveValidation>());

            handler.ReviewFailedValidation(0, "");
            handler.Handle(new Request<PostedReview>(new PostedReview()));

            Substitute.For<IObserveReview>()
                .DidNotReceiveWithAnyArgs()
                .ReviewReadyForSaving(null);
        }

        [Test]
        public void Notifies_with_http_415_and_error_message_when_content_type_is_incorrect()
        {
            var savingObserver = Substitute.For<IObserveValidation>();

            var handler = new ReviewHandler(Substitute.For<IObserveReview>(), savingObserver);

            var headers = new Dictionary<string, string>
            {
                ["Content-type"] = "xxxxxxx"
            };

            handler.Handle(new Request<PostedReview>(new PostedReview(), headers));

            savingObserver
                .Received()
                .ReviewFailedValidation(415, "Incorrect content type");
        }

        [TestCase("")]
        [TestCase("just some text")]
        [TestCase("ftp://company.com/folder")]
        //etc...
        public void Notifies_with_http_400_and_error_message_when_referer_has_invalid_uri_format(string referer)
        {
            var savingObserver = Substitute.For<IObserveValidation>();

            var handler = new ReviewHandler(Substitute.For<IObserveReview>(), savingObserver);

            var headers = new Dictionary<string, string>
            {
                ["Referer"] = referer,
                ["Content-type"] = "application/json"
            };

            handler.Handle(new Request<PostedReview>(new PostedReview(), headers));

            savingObserver
                .Received()
                .ReviewFailedValidation(400, "Bad referer uri");
        }

        [TestCase("123456789012")]
        [TestCase("12345678901234")]
        [TestCase("XXXXXXXXXXXXXX")]
        public void Notifies_with_http_400_and_error_message_when_isbn_is_invalid(string isbn)
        {
            var savingObserver = Substitute.For<IObserveValidation>();
            var handler = new ReviewHandler(Substitute.For<IObserveReview>(), savingObserver);

            var postedReview = new PostedReview
            {
                ISBN = isbn
            };

            handler.Handle(new Request<PostedReview>(postedReview, _validHeaders));

            savingObserver
                .Received()
                .ReviewFailedValidation(400, "Invalid ISBN");
        }
    }
}