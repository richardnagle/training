using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        public Response Handle(Request<PostedReview> request)
        {
            var headers = new HttpHeaders(request.Headers);
            var isbn = new Isbn(request.Body.ISBN);

            var validators = new IValidateAReview[] {headers.ContentType, headers.Referer, isbn};

            foreach (var validator in validators)
            {
                if (validator.IsInvalid())
                {
                    return validator.GetErrorResponse();
                }
            }

            return new Response(201, string.Empty);
        }
    }
}