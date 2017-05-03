using System.IO;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class RequestRepository: IObserveReview
    {
        public static string DatabaseFile =>
            @"c:\_root\training\behavioural-testing\code\reviews-service\bin\debug\reviews.db";

        public void ReviewReadyForSaving(FormattedReview formattedReview)
        {
            var dto = new ReviewDto();
            Save(dto);
        }

        private void Save(ReviewDto dto)
        {
            var row = $"{dto.ISBN}|{dto.Reviewer}|{dto.Text}|{dto.Uri}";

            File.AppendAllText(DatabaseFile, row);
        }
    }
}