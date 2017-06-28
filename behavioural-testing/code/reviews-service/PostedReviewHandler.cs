using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        public Response Handle(Request<PostedReview> request)
        {
            var headers = new HttpHeaders(request.Headers);
            var isbn = new Isbn(request.Body.ISBN);

            if (headers.ContentType.IsInvalid())
            {
                return headers.ContentType.GetErrorReponse();
            }

            if (headers.Referer.IsInvalid())
            {
                return headers.Referer.GetErrorResponse();
            }

            if (isbn.IsInvalid())
            {
                return isbn.GetErrorResponse();
            }

            return new Response(201, string.Empty);
        }
    }
}