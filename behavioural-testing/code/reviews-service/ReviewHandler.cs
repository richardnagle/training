﻿using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>, IObserveSaving
    {
        private readonly IObserveReview _reviewObserver;
        private readonly IObserveSaving _savingObserver;

        public ReviewHandler(IObserveReview reviewObserver, IObserveSaving savingObserver)
        {
            _reviewObserver = reviewObserver;
            _savingObserver = savingObserver;
        }

        private bool _proceedToSave = true;

        public void Handle(Request<PostedReview> request)
        {
            var headers = new Headers(request.Headers);
            var formattedReview = FormattedReview.FromPosted(request.Body, headers);

            headers.Validate(_savingObserver);
            formattedReview.Validate(_savingObserver);

            if (_proceedToSave)
            {
                _reviewObserver.ReviewReadyForSaving(formattedReview);
            }
        }

        public void ReviewNotSaved(int httpStatusCode, string errorMessage)
        {
            _proceedToSave = false;
        }

        public void ReviewSaved()
        {}
    }
}