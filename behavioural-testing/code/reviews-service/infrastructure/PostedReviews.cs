namespace reviews_service.infrastructure
{
    public class PostedReviews
    {
        public Book[] Books { get; set; }
    }

    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public BookReview[] Reviews { get; set; }
    }

    public class BookReview
    {
        public string Reviewer { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
    }
}
