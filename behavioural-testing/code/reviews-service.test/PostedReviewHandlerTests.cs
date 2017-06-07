using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    [TestFixture]
    public class PostedReviewHandlerTests
    {
        [Test]
        public void Responds_with_http_415_when_content_type_header_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(false);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator, Substitute.For<IReviewDtoMapper>(), Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(415));
            Assert.That(response.Error, Is.EqualTo("Incorrect content type"));
        }

        [Test]
        public void Responds_with_http_400_when_referer_uri_is_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(false);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator, Substitute.For<IReviewDtoMapper>(), Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Bad referer uri"));
        }

        [Test]
        public void Responds_with_http_400_when_isbn_is_invalid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(false);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator, Substitute.For<IReviewDtoMapper>(), Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(400));
            Assert.That(response.Error, Is.EqualTo("Invalid ISBN"));
        }

        [Test]
        public void Responds_with_http_201_when_request_is_valid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var handler = new PostedReviewHandler(Substitute.For<ISaveReviews>(), validator, Substitute.For<IReviewDtoMapper>(), Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            var response = handler.Handle(request);

            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Error, Is.Empty);
        }

        [Test]
        public void Saves_review_when_request_is_valid()
        {
            var request = new Request<PostedReview>(new PostedReview());

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var databaseService = Substitute.For<ISaveReviews>();

            var handler = new PostedReviewHandler(databaseService, validator, Substitute.For<IReviewDtoMapper>(), Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            handler.Handle(request);

            databaseService.Received().Insert(Arg.Any<ReviewDto>());
        }

        [Test]
        public void Saved_review_has_values_mapped_from_http_header()
        {
            var httpHeaders = new Dictionary<string, string>();
            var request = new Request<PostedReview>(new PostedReview(), httpHeaders);

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var databaseService = Substitute.For<ISaveReviews>();

            ReviewDto mappedDto = null;
            var mapper = Substitute.For<IReviewDtoMapper>();
            mapper.MapHttpHeaders(httpHeaders, Arg.Do<ReviewDto>(arg => mappedDto = arg));

            var handler = new PostedReviewHandler(databaseService, validator, mapper, Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            handler.Handle(request);

            mapper.Received().MapHttpHeaders(httpHeaders, mappedDto);
            databaseService.Received().Insert(mappedDto);
        }

        [Test]
        public void Saved_review_has_isbn_and_reviewer_mapped_from_body()
        {
            var postedReview = new PostedReview();

            var request = new Request<PostedReview>(postedReview);

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var databaseService = Substitute.For<ISaveReviews>();

            ReviewDto mappedDto = null;
            var mapper = Substitute.For<IReviewDtoMapper>();
            mapper.MapBodyFields(postedReview, Arg.Do<ReviewDto>(arg => mappedDto = arg));

            var handler = new PostedReviewHandler(databaseService, validator, mapper, Substitute.For<IReviewHtmlFormatter>(), Substitute.For<ISectionWalker>());
            handler.Handle(request);

            mapper.Received().MapBodyFields(postedReview, mappedDto);
            databaseService.Received().Insert(mappedDto);
        }

        [Test]
        public void Saved_review_has_sections_formatted_as_html()
        {
            var sections = new ReviewSection[0];
            var postedReview = new PostedReview {Sections = sections};

            var request = new Request<PostedReview>(postedReview);

            var validator = Substitute.For<IReviewValidator>();
            validator.ValidateContentType(request).Returns(true);
            validator.ValidateReferer(request).Returns(true);
            validator.ValidateISBN(request).Returns(true);

            var databaseService = Substitute.For<ISaveReviews>();

            const string title = "the header";
            const string subTitle = "the sub-header";
            const string content = "the content";

            var sectionWalker = Substitute.For<ISectionWalker>();
            sectionWalker.GetText(sections, "Title").Returns(title);
            sectionWalker.GetText(sections, "SubTitle").Returns(subTitle);
            sectionWalker.GetText(sections, "Body").Returns(content);

            const string htmlFormattedText = "<some html>";
            var htmlFormatter = Substitute.For<IReviewHtmlFormatter>();
            htmlFormatter.Format(title, subTitle, content).Returns(htmlFormattedText);

            ReviewDto mappedDto = null;
            var mapper = Substitute.For<IReviewDtoMapper>();
            mapper.MapText(htmlFormattedText, Arg.Do<ReviewDto>(arg => mappedDto = arg));

            var handler = new PostedReviewHandler(databaseService, validator, mapper, htmlFormatter, sectionWalker);
            handler.Handle(request);

            mapper.Received().MapBodyFields(postedReview, mappedDto);
            databaseService.Received().Insert(mappedDto);
        }
    }
}