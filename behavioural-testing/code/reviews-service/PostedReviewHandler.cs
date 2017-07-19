using System.Linq;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        private readonly ISaveReviews _database;

        public PostedReviewHandler(ISaveReviews database)
        {
            _database = database;
        }

        public Response Handle(Request<PostedReview> request)
        {
            var postedReview = request.Body;
            var headers = new HttpHeaders(request.Headers);
            var isbn = new Isbn(postedReview.ISBN);

            var validators = new IValidateAReview[] {headers.ContentType, headers.Referer, isbn};

            foreach (var validator in validators)
            {
                if (validator.IsInvalid())
                {
                    return validator.GetErrorResponse();
                }
            }

            var h1Text = postedReview.Sections.First(sect => sect.Name == "title").Text;
            var h2Text = postedReview.Sections.First(sect => sect.Name == "subtitle").Text;
            var pText = postedReview.Sections.First(sect => sect.Name == "body").Text;

            var reviewDto = new ReviewDto
            {
                ISBN = long.Parse(postedReview.ISBN),
                Reviewer = postedReview.Reviewer,
                Uri = request.Headers["Referer"],
                Text = $"<h1>{h1Text}</h1>\r\n<h2>{h2Text}</h2>\r\n<p>{pText}</p>"
            };

            _database.Insert(reviewDto);

            return new Response(201, string.Empty);
        }
    }
}