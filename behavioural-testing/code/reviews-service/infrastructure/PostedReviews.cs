using System;

namespace reviews_service.infrastructure
{
    public class PostedReviews
    {
        public DateTime WhenPosted { get; set; }
        public Book Book { get; set; }
        public BookReview Review { get; set; }
    }

    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
    }

    public class BookReview
    {
        public string Reviewer { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
    }
}
