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

            var reviewDto = new ReviewDto();
            isbn.Populate(reviewDto);
            headers.Referer.Populate(reviewDto);
            htmlBody.Populate(reviewDto);
            reviewer.Populate(reviewDto);

            _database.Insert(reviewDto);

            return new Response(201, string.Empty);
        }
    }
}