namespace reviews_service.infrastructure
{
    public interface ISaveReviews
    {
        void Insert(ReviewDto dto);
    }
}
