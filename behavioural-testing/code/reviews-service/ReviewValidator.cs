using System.Text.RegularExpressions;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewValidator
    {
        public bool ValidateContentType(Request<PostedReview> request)
        {
            return request.Headers["Content-type"] == "application/json";
        }

        public bool ValidateReferer(Request<PostedReview> request)
        {
            return Regex.IsMatch(request.Headers["Referer"], "http(s)?://(.*).(.*)");
        }

        public bool ValidateISBN(Request<PostedReview> request)
        {
            return Regex.IsMatch(request.Body.ISBN, @"^\d{13}$");
        }
    }
}