using reviews_service.infrastructure;

namespace reviews_service
{
    public interface IPopulateReviewDto
    {
        void Populate(ReviewDto reviewDto);
    }
}