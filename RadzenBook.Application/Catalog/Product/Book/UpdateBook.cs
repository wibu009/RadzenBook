using RadzenBook.Application.Common.Photo;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class UpdateBookRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public string? Language { get; set; }
    public string? Translator { get; set; }
    public int PageCount { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public CurrencyUnit Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public int Republish { get; set; }
    public CoverType CoverType { get; set; }
    public DateTime? PublishDate { get; set; }
    public Guid? AuthorId { get; set; }
    public List<Guid>? GenreIds { get; set; }
    public List<ProductImageDto>? Images { get; set; }
    public List<IFormFile>? NewImages { get; set; }
    public Guid? PublisherId { get; set; }
    public Guid CategoryId { get; set; }
}

public class UpdateBookRequestValidator : CustomValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator(IStringLocalizer<UpdateBookRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage(t["Title must not exceed {0} characters", 200]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.ISBN)
            .MaximumLength(50).WithMessage(t["ISBN must not exceed {0} characters", 50]);
        RuleFor(x => x.Language)
            .MaximumLength(50).WithMessage(t["Language must not exceed {0} characters", 50]);
        RuleFor(x => x.Translator)
            .MaximumLength(50).WithMessage(t["Translator must not exceed {0} characters", 50]);
        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage(t["PageCount must be greater than {0}", 0]);
        RuleFor(x => x.ImportPrice)
            .GreaterThan(0).WithMessage(t["ImportPrice must be greater than {0}", 0]);
        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage(t["SalePrice must be greater than {0}", 0]);
        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage(t["UnitPrice must be greater than {0}", 0]);
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage(t["Weight must be greater than {0}", 0]);
        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage(t["Width must be greater than {0}", 0]);
        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage(t["Height must be greater than {0}", 0]);
        RuleFor(x => x.Depth)
            .GreaterThan(0).WithMessage(t["Depth must be greater than {0}", 0]);
    }
}

public class UpdateBookRequestHandler : IRequestHandler<UpdateBookRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly ILogger<CreateBookRequestHandler> _logger;

    public UpdateBookRequestHandler(
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

    public async Task<Result<Unit>> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the existing product
            var product = await _unitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (product == null)
            {
                return Result<Unit>.Failure(_t["Product does not exist"]);
            }

            // Update product properties
            _mapper.Map(request, product);
            product.ModifiedBy = _userAccessor.GetUsername();

            // Get the existing book
            var book = await _unitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (book == null)
            {
                return Result<Unit>.Failure(_t["Book does not exist"]);
            }

            // Update book properties
            _mapper.Map(request, book);
            book.ModifiedBy = _userAccessor.GetUsername();

            // Update images
            await UpdateImages(request, product, cancellationToken);

            // Update genres
            await UpdateGenres(request, book, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Book updated successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateBookRequestHandler), e.Message, e);
        }
    }

    private async Task UpdateImages(UpdateBookRequest request, Domain.Catalog.Product product,
        CancellationToken cancellationToken)
    {
        // Add new images if they are provided
        if (request.NewImages is { Count: > 0 })
        {
            var newImages = await _photoAccessor.AddRangePhotoAsync(request.NewImages);
            var imagesToAdd = _mapper.Map<List<ProductImage>>(newImages);
            await _unitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
                .CreateRangeAsync(imagesToAdd, cancellationToken);
        }

        // Retrieve the existing images from the database
        var existingImages = await _unitOfWork
            .GetRepository<IProductImageRepository, ProductImage, string>()
            .GetAsync(x => x.ProductId == product.Id, cancellationToken: cancellationToken);

        // Convert the existing images and request images to sets for easy comparison
        var requestImageIds = new HashSet<string>(request.Images!.Select(x => x.Id)!);

        var imagesToDelete = existingImages.Where(x => !requestImageIds.Contains(x.Id)).ToList();

        // Delete removed images
        await _photoAccessor.DeleteRangePhotoAsync(imagesToDelete.Select(x => x.Id).ToList());
        await _unitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
            .DeleteRangeAsync(imagesToDelete, cancellationToken);
    }

    private async Task UpdateGenres(UpdateBookRequest request, Domain.Catalog.Book book,
        CancellationToken cancellationToken)
    {
        // Retrieve the existing genres from the database
        var existingGenres = await _unitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
            .GetAsync(x => x.BookId == book.Id, cancellationToken: cancellationToken);

        // Convert the existing genres and request genres to sets for easy comparison
        var requestGenreIds = new HashSet<Guid>(request.GenreIds ?? new List<Guid>());

        // Find the genres that need to be added and deleted
        var genresToAdd = request.GenreIds?.Where(x => existingGenres.All(g => g.GenreId != x)).ToList() ??
                          new List<Guid>();
        var genresToDelete = existingGenres.Where(x => !requestGenreIds.Contains(x.GenreId)).ToList();

        // Add new genres
        var newGenres = genresToAdd.Select(genreId => new BookGenre
        {
            BookId = book.Id,
            GenreId = genreId
        }).ToList();

        await _unitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
            .CreateRangeAsync(newGenres, cancellationToken);

        // Delete removed genres
        await _unitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
            .DeleteRangeAsync(genresToDelete, cancellationToken);
    }
}