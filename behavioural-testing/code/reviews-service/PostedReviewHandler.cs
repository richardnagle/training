using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        private readonly ISaveReviews _database;

        public PostedReviewHandler(ISaveReviews database)
        {
            _database = database;
        }

        public Response Handle(Request<PostedReview> request)
        {
            var postedReview = request.Body;
            var headers = new HttpHeaders(request.Headers);
            var isbn = new Isbn(postedReview.ISBN);
            var htmlBody = new HtmlBody(postedReview.Sections);
            var reviewer = new Reviewer(postedReview.Reviewer);

            var validators = new IValidateAReview[] {headers.ContentType, headers.Referer, isbn};

            foreach (var validator in validators)
            {
                if (validator.IsInvalid())
                {
                    return validator.GetErrorResponse();
                }
            }

            var populators = new IPopulateReviewDto[] {isbn, headers.Referer, htmlBody, reviewer};

            var reviewDto = new ReviewDto();

            foreach (var populator in populators)
            {
                populator.Populate(reviewDto);
            }

            _database.Insert(reviewDto);

            return new Response(201, string.Empty);
        }
    }
}