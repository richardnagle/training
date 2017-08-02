namespace reviews_service.infrastructure
{
    public class PostedReview
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Reviewer { get; set; }
        public ReviewSection[] Sections { get; set; }
    }

    public class ReviewSection
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}