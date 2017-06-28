using System.Text.RegularExpressions;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        public Response Handle(Request<PostedReview> request)
        {
            var headers = new HttpHeaders(request.Headers);

            if (headers.ContentType != "application/json")
            {
                return new Response(415, "Incorrect content type");
            }

            if (!Regex.IsMatch(headers.Referer, "http(s)?://(.*).(.*)"))
            {
                return new Response(400, "Bad referer uri");
            }

            if (!Regex.IsMatch(request.Body.ISBN, @"^\d{13}$"))
            {
                return new Response(400, "Invalid isbn");
            }

            return new Response(201, string.Empty);
        }
    }
}