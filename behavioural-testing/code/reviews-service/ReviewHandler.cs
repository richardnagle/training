using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>, IObserveValidation
    {
        private readonly IObserveReview _reviewObserver;
        private readonly IObserveValidation _validationObserver;

        public ReviewHandler(IObserveReview reviewObserver, IObserveValidation validationObserver)
        {
            _reviewObserver = reviewObserver;
            _validationObserver = validationObserver;
        }

        private bool _proceedToSave = true;

        public void Handle(Request<PostedReview> request)
        {
            var headers = new Headers(request.Headers);
            var formattedReview = FormattedReview.FromPosted(request.Body, headers);

            headers.Validate(_validationObserver);
            formattedReview.Validate(_validationObserver);

            if (_proceedToSave)
            {
                _reviewObserver.ReviewReadyForSaving(formattedReview);
            }
        }

        public void ReviewFailedValidation(int httpStatusCode, string errorMessage)
        {
            _proceedToSave = false;
        }
    }
}