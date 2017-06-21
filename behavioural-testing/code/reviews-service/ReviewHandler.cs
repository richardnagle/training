using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>
    {
        public Response Handle(Request<PostedReview> request)
        {
            return new Response(201,"");
        }
    }
}