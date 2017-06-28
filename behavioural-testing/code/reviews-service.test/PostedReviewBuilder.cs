using System.Collections.Generic;
using reviews_service.infrastructure;

namespace reviews_service.test
{
    public class PostedReviewBuilder
    {
        private string _isbn = "1234567890123";
        private string _bodyTitle = "the-title";
        private string _bodySubTitle = "the-sub-title";
        private string _bodyText = "the-body";
        private string _refererUrl = "http://the-referer.com";
        private string _contentType = "application/json";

        public PostedReviewBuilder WithIsbn(string isbn)
        {
            _isbn = isbn;
            return this;
        }

        public PostedReviewBuilder WithBodyTitle(string bodyTitle)
        {
            _bodyTitle = bodyTitle;
            return this;
        }

        public PostedReviewBuilder WithBodySubTitle(string bodySubTitle)
        {
            _bodySubTitle = bodySubTitle;
            return this;
        }

        public PostedReviewBuilder WithBodyText(string bodyText)
        {
            _bodyText = bodyText;
            return this;
        }

        public PostedReviewBuilder WithRefererUrl(string refererUrl)
        {
            _refererUrl = refererUrl;
            return this;
        }

        public PostedReviewBuilder WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public Request<PostedReview> Build()
        {
            return new Request<PostedReview>(
                new PostedReview
                {
                    ISBN = _isbn,
                    Sections = new[]
                    {
                        new ReviewSection {Name = "title", Text = _bodyTitle},
                        new ReviewSection {Name = "subtitle", Text = _bodySubTitle},
                        new ReviewSection {Name = "body", Text = _bodyText},
                    }
                },
                new Dictionary<string, string>()
                {
                    ["Content-type"] = _contentType,
                    ["Referer"] = _refererUrl
                });
        }
    }
}