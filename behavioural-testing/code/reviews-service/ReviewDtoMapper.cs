using System.Collections.Generic;
using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewDtoMapper
    {
        public void MapHttpHeaders(IDictionary<string, string> httpHeaders, ReviewDto reviewDto)
        {
            reviewDto.Uri = httpHeaders["Referer"];
        }

        public void MapBodyFields(PostedReview postedReview, ReviewDto reviewDto)
        {
            reviewDto.ISBN = long.Parse(postedReview.ISBN);
            reviewDto.Reviewer = postedReview.Reviewer;
        }

        public void MapText(string htmlFormattedText, ReviewDto reviewDto)
        {
            reviewDto.Text = htmlFormattedText;
        }
    }
}
