using System.Collections.Generic;
using reviews_service.infrastructure;

namespace reviews_service
{
    public interface IReviewDtoMapper
    {
        void MapHttpHeaders(IDictionary<string, string> httpHeaders, ReviewDto reviewDto);
        void MapBodyFields(PostedReview postedReview, ReviewDto reviewDto);
        void MapText(string htmlFormattedText, ReviewDto reviewDto);
    }
}