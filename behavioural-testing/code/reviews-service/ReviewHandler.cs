using reviews_service.infrastructure;

namespace reviews_service
{
    public class ReviewHandler: IHandle<PostedReview>
    {
        private readonly ReviewDtoMapper _dtoMapper;
        private readonly SectionWalker _sectionWalker;
        private readonly ReviewHtmlFormatter _htmlFormatter;
        private readonly ISaveReviews _database;
        private readonly ReviewValidator _reviewValidator;

        public ReviewHandler(ReviewDtoMapper dtoMapper, SectionWalker sectionWalker, ReviewHtmlFormatter htmlFormatter, ISaveReviews database, ReviewValidator reviewValidator)
        {
            _dtoMapper = dtoMapper;
            _sectionWalker = sectionWalker;
            _htmlFormatter = htmlFormatter;
            _database = database;
            _reviewValidator = reviewValidator;
        }

        public Response Handle(Request<PostedReview> request)
        {
            if (!_reviewValidator.ValidateContentType(request))
            {
                return new Response(415, "Incorrect content type");
            }

            if (!_reviewValidator.ValidateReferer(request))
            {
                return new Response(400, "Bad referer uri");
            }

            if (!_reviewValidator.ValidateISBN(request))
            {
                return new Response(400, "Invalid ISBN");
            }

            var title = _sectionWalker.GetText(request.Body.Sections, "title");
            var subTitle = _sectionWalker.GetText(request.Body.Sections, "subtitle");
            var body = _sectionWalker.GetText(request.Body.Sections, "body");
            var bodyHtml = _htmlFormatter.Format(title, subTitle, body);

            var dto = new ReviewDto();

            _dtoMapper.MapBodyFields(request.Body, dto);
            _dtoMapper.MapText(bodyHtml, dto);
            _dtoMapper.MapHttpHeaders(request.Headers, dto);

            _database.Insert(dto);

            return new Response(201,"");
        }
    }
}