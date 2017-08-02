namespace reviews_service.infrastructure
{
    public class ReviewDto
    {
        public long ISBN { get; set; }
        public string Reviewer { get; set; }
        public string Uri { get; set; }
        public string Text { get; set; }
    }
}