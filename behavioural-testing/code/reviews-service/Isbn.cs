using System.Text.RegularExpressions;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class Isbn: IValidateAReview, IPopulateReviewDto
    {
        private readonly string _value;

        public Isbn(string value)
        {
            _value = value;
        }

        public bool IsInvalid()
        {
            return !Regex.IsMatch(_value, @"^\d{13}$");
        }

        public Response GetErrorResponse()
        {
            return new Response(400, "Invalid isbn");
        }

        public void Populate(ReviewDto reviewDto)
        {
            reviewDto.ISBN = long.Parse(_value);
        }
    }
}