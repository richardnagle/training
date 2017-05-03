using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>
    {
        private readonly IObserveReview _reviewObserver;

        public ReviewHandler(IObserveReview reviewObserver)
        {
            _reviewObserver = reviewObserver;
        }

        public Response Handle(Request<PostedReview> request)
        {
            var headers = new Headers(request.Headers);
            _reviewObserver.ReviewReadyForSaving(FormattedReview.FromPosted(request.Body, headers));
            return new Response(0,"");
        }
    }
}