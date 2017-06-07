using reviews_service.infrastructure;

namespace reviews_service
{
    public class PostedReviewHandler: IHandle<PostedReview>
    {
        private readonly ISaveReviews _databaseService;
        private readonly IReviewValidator _validator;
        private readonly IReviewDtoMapper _mapper;
        private readonly IReviewHtmlFormatter _htmlFormatter;
        private readonly ISectionWalker _sectionWalker;

        public PostedReviewHandler(
            ISaveReviews databaseService,
            IReviewValidator validator,
            IReviewDtoMapper mapper,
            IReviewHtmlFormatter htmlFormatter,
            ISectionWalker sectionWalker)
        {
            _databaseService = databaseService;
            _validator = validator;
            _mapper = mapper;
            _htmlFormatter = htmlFormatter;
            _sectionWalker = sectionWalker;
        }

        public Response Handle(Request<PostedReview> request)
        {
            if (!_validator.ValidateContentType(request))
            {
                return new Response(415, "Incorrect content type");
            }

            if (!_validator.ValidateReferer(request))
            {
                return new Response(400, "Bad referer uri");
            }

            if (!_validator.ValidateISBN(request))
            {
                return new Response(400, "Invalid ISBN");
            }

            var reviewDto = new ReviewDto();

            var title = _sectionWalker.GetText(request.Body.Sections, "Title");
            var subTitle = _sectionWalker.GetText(request.Body.Sections, "SubTitle");
            var body = _sectionWalker.GetText(request.Body.Sections, "Body");

            var text = _htmlFormatter.Format(title, subTitle, body);

            _mapper.MapHttpHeaders(request.Headers, reviewDto);
            _mapper.MapBodyFields(request.Body, reviewDto);
            _mapper.MapText(text, reviewDto);

            _databaseService.Insert(reviewDto);

            return new Response(201, "");
        }
    }
}
