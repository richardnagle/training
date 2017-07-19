using reviews_service.infrastructure;

namespace reviews_service
{
    public class Reviewer
    {
        private readonly string _value;

        public Reviewer(string value)
        {
            _value = value;
        }

        public void Populate(ReviewDto reviewDto)
        {
            reviewDto.Reviewer = _value;
        }
    }
}