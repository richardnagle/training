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
            var review = new Review(request.Body, new HttpHeaders(request.Headers));

            if (review.IsInvalid())
            {
                return review.GetErrorResponse();
            }

            _database.Insert(review.ToReviewDto());

            return new Response(201, string.Empty);
        }
    }
}