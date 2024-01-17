using RadzenBook.Application.Common.Photo;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class CreateBookRequest : IRequest<Result<Unit>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public string? Language { get; set; }
    public string? Translator { get; set; }
    public int PageCount { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string? Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public int Republish { get; set; }
    public CoverType CoverType { get; set; }
    public DateTime? PublishDate { get; set; }
    public Guid? AuthorId { get; set; }
    public List<Guid>? GenreIds { get; set; }
    public List<IFormFile>? Images { get; set; }
    public Guid? PublisherId { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreateBookRequestValidator : CustomValidator<CreateBookRequest>
{
    public CreateBookRequestValidator(IStringLocalizer<CreateBookRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(t["Title is required"])
            .MaximumLength(200).WithMessage(t["Title must not exceed {0} characters", 200]);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(t["Description is required"])
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage(t["ISBN is required"])
            .MaximumLength(50).WithMessage(t["ISBN must not exceed {0} characters", 50]);
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage(t["Language is required"])
            .MaximumLength(50).WithMessage(t["Language must not exceed {0} characters", 50]);
        RuleFor(x => x.Translator)
            .MaximumLength(50).WithMessage(t["Translator must not exceed {0} characters", 50]);
        RuleFor(x => x.PageCount)
            .NotEmpty().WithMessage(t["PageCount is required"])
            .GreaterThan(0).WithMessage(t["PageCount must be greater than {0}", 0]);
        RuleFor(x => x.ImportPrice)
            .NotEmpty().WithMessage(t["ImportPrice is required"])
            .GreaterThan(0).WithMessage(t["ImportPrice must be greater than {0}", 0]);
        RuleFor(x => x.SalePrice)
            .NotEmpty().WithMessage(t["SalePrice is required"])
            .GreaterThan(0).WithMessage(t["SalePrice must be greater than {0}", 0]);
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage(t["Currency is required"]);
        RuleFor(x => x.UnitPrice)
            .NotEmpty().WithMessage(t["UnitPrice is required"])
            .GreaterThan(0).WithMessage(t["UnitPrice must be greater than {0}", 0]);
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(t["Quantity is required"])
            .GreaterThanOrEqualTo(0).WithMessage(t["Quantity must be greater than or equal to {0}", 0]);
        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage(t["Weight is required"])
            .GreaterThan(0).WithMessage(t["Weight must be greater than {0}", 0]);
        RuleFor(x => x.Width)
            .NotEmpty().WithMessage(t["Width is required"])
            .GreaterThan(0).WithMessage(t["Width must be greater than {0}", 0]);
        RuleFor(x => x.Height)
            .NotEmpty().WithMessage(t["Height is required"])
            .GreaterThan(0).WithMessage(t["Height must be greater than {0}", 0]);
        RuleFor(x => x.Depth)
            .NotEmpty().WithMessage(t["Depth is required"])
            .GreaterThan(0).WithMessage(t["Depth must be greater than {0}", 0]);
        RuleFor(x => x.Republish)
            .NotEmpty().WithMessage(t["Republish is required"]);
        RuleFor(x => x.CoverType)
            .NotEmpty().WithMessage(t["CoverType is required"]);
        RuleFor(x => x.PublishDate)
            .NotEmpty().WithMessage(t["PublishDate is required"]);
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage(t["AuthorId is required"]);
        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage(t["PublisherId is required"]);
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(t["CategoryId is required"]);
        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage(t["GenreIds is required"]);
        RuleFor(x => x.Images)
            .NotEmpty().WithMessage(t["Images is required"]);
    }
}

public class CreateBookRequestHandler : IRequestHandler<CreateBookRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly ILogger<CreateBookRequestHandler> _logger;

    public CreateBookRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateBookRequestHandler>();
        _t = t.Create(typeof(CreateBookRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            //Add product
            var product = _mapper.Map<Domain.Catalog.Product>(request);
            product.CreatedBy = _userAccessor.GetUsername();
            product.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .CreateAsync(product, cancellationToken);

            //Add book
            var book = _mapper.Map<Domain.Catalog.Book>(request);
            book.ProductId = product.Id;
            book.CreatedBy = _userAccessor.GetUsername();
            book.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .CreateAsync(book, cancellationToken);

            //Add images
            var images = _photoAccessor.AddRangePhotoAsync(request.Images ?? new List<IFormFile>()).Result;
            if (images.Count != request.Images?.Count)
                return Result<Unit>.Failure(_t["Image upload failed"]);
            await _unitOfWork.GetRepository<IProductImageRepository, Domain.Catalog.ProductImage, string>()
                .CreateRangeAsync(images.Select(x => new Domain.Catalog.ProductImage
                {
                    Id = x.PublicId,
                    ProductId = product.Id,
                    ImageUrl = x.Url
                }), cancellationToken);

            //Add genres
            var genreIds = request.GenreIds ?? new List<Guid>();
            var genres = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetAsync(x => genreIds.Contains(x.Id), cancellationToken: cancellationToken);
            if (genres.Count != genreIds.Count)
                return Result<Unit>.Failure(_t["Genre not found"]);
            await _unitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
                .CreateRangeAsync(genres.Select(x => new BookGenre
                {
                    BookId = book.Id,
                    GenreId = x.Id
                }), cancellationToken);

            //Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Book created successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateBookRequestHandler), e.Message, e);
        }
    }
}