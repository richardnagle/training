using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewRepository: IObserveReview
    {
        private readonly ISaveReviews _database;
        private readonly IObserveSaving _savingObserver;

        public ReviewRepository(ISaveReviews database, IObserveSaving savingObserver)
        {
            _database = database;
            _savingObserver = savingObserver;
        }

        public void ReviewReadyForSaving(FormattedReview formattedReview)
        {
            var dto = new ReviewDto
            {
                ISBN = formattedReview.ISBN,
                Reviewer = formattedReview.Reviewer,
                Uri = formattedReview.Uri,
                Text = formattedReview.Text
            };

            _database.Insert(dto);

            _savingObserver.ReviewSaved();
        }
    }
}