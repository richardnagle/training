using System.Collections.Generic;
using System.Linq;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class Review
    {
        private readonly ContentType _contentType;
        private readonly Referer _referer;
        private readonly Isbn _isbn;
        private readonly HtmlBody _body;
        private readonly Reviewer _reviewer;

        public Review(PostedReview postedReview, HttpHeaders headers)
        {
            _contentType = headers.ContentType;
            _referer = headers.Referer;
            _isbn = new Isbn(postedReview.ISBN);
            _body = new HtmlBody(postedReview.Sections);
            _reviewer = new Reviewer(postedReview.Reviewer);
        }

        public bool IsInvalid()
        {
            return GetValidators().Any(x => x.IsInvalid());
        }

        public Response GetErrorResponse()
        {
            return GetValidators().FirstOrDefault(x => x.IsInvalid())?.GetErrorResponse();
        }

        private IEnumerable<IValidateAReview> GetValidators()
        {
            yield return _contentType;
            yield return _referer;
            yield return _isbn;
        }

        public ReviewDto ToReviewDto()
        {
            var reviewDto = new ReviewDto();

            foreach (var populator in GetReviewDtoPopulators())
            {
                populator.Populate(reviewDto);
            }

            return reviewDto;
        }

        private IEnumerable<IPopulateReviewDto> GetReviewDtoPopulators()
        {
            yield return _isbn;
            yield return _referer;
            yield return _body;
            yield return _reviewer;
        }
    }
}