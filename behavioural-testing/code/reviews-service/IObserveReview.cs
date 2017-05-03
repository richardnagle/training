namespace reviews_service
{
    public interface IObserveReview
    {
        void ReviewReadyForSaving(FormattedReview formattedReview);
    }
}