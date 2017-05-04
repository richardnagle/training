using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewRepository: IObserveReview
    {
        private readonly ISaveReviews _database;

        public ReviewRepository(ISaveReviews database)
        {
            _database = database;
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
        }
    }
}