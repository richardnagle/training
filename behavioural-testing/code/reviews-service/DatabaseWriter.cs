using System.IO;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class DatabaseWriter: ISaveReviews
    {
        public const string DatabaseFile =
            @"c:\_root\training\behavioural-testing\code\reviews-service\bin\debug\reviews.db";

        public void Insert(ReviewDto dto)
        {
            var row = $"{dto.ISBN}|{dto.Reviewer}|{dto.Text}|{dto.Uri}";

            File.AppendAllText(DatabaseFile, row);
        }
    }
}