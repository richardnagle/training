using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        private readonly ISaveReviews _databaseService;
        private readonly IReviewValidator _validator;

        public PostedReviewHandler(ISaveReviews databaseService, IReviewValidator validator)
        {
            _databaseService = databaseService;
            _validator = validator;
        }

        public Response Handle(Request<PostedReview> request)
        {
            if (!_validator.ValidateContentType(request))
            {
                return new Response(415, "Incorrect content type");
            }

            if (!_validator.ValidateReferer(request))
            {
                return new Response(400, "Bad referer uri");
            }

            if (!_validator.ValidateISBN(request))
            {
                return new Response(400, "Invalid ISBN");
            }

            _databaseService.Insert(new ReviewDto());

            return new Response(201, "");
        }
    }
}
