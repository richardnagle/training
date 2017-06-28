using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        public Response Handle(Request<PostedReview> request)
        {
            return new Response(415,"Incorrect content type");
        }
    }
}