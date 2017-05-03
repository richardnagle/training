using System.IO;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>
    {
        public static string DatabaseFile =>
            @"c:\_root\training\behavioural-testing\code\reviews-service\bin\debug\reviews.db";


        public Response Handle(Request<PostedReview> request)
        {
            var dto = new ReviewDto();
            Save(dto);
            return new Response(0,"");
        }

        private void Save(ReviewDto dto)
        {
            var row = $"{dto.ISBN}|{dto.Reviewer}|{dto.Text}|{dto.Uri}";

            File.AppendAllText(DatabaseFile, row);
        }
    }
}