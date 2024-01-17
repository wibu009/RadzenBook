using RadzenBook.Application.Common.Photo;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class CreateBookRequest : CreateProductRequest
{
    public string? ISBN { get; set; }
    public string? Language { get; set; }
    public string? Translator { get; set; }
    public int PageCount { get; set; }
    public double Weight { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public int Republish { get; set; }
    public CoverType CoverType { get; set; }
    public DateTime? PublishDate { get; set; }
    public Guid? AuthorId { get; set; }
    public List<Guid>? GenreIds { get; set; }
    public Guid? PublisherId { get; set; }
}

public class CreateBookRequestValidator : CreateProductRequestValidator<CreateBookRequest>
{
    public CreateBookRequestValidator(IStringLocalizer<CreateBookRequestValidator> t) : base(t)
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage(t["ISBN is required"])
            .MaximumLength(50).WithMessage(t["ISBN must not exceed {0} characters", 50]);
        RuleFor(x => x.Language)
            .NotEmpty().WithMessage(t["Language is required"])
            .MaximumLength(50).WithMessage(t["Language must not exceed {0} characters", 50]);
        RuleFor(x => x.Translator)
            .MaximumLength(50).WithMessage(t["Translator must not exceed {0} characters", 50]);
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
        RuleFor(x => x.GenreIds)
            .NotEmpty().WithMessage(t["GenreIds is required"]);
    }
}

public class CreateBookRequestHandler : CreateProductRequestHandler<CreateBookRequest>
{
    public CreateBookRequestHandler(IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IStringLocalizerFactory t, 
        IInfrastructureServiceManager infrastructureServiceManager, 
        ILoggerFactory loggerFactory) 
        : base(unitOfWork, mapper, t, infrastructureServiceManager, loggerFactory) { }

    public override async Task<Result<Unit>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            //Add product
            var product = Mapper.Map<Domain.Catalog.Product>(request);
            product.CreatedBy = UserAccessor.GetUsername();
            product.ModifiedBy = UserAccessor.GetUsername();
            await UnitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .CreateAsync(product, cancellationToken);

            //Add book
            var book = Mapper.Map<Domain.Catalog.Book>(request);
            book.ProductId = product.Id;
            book.CreatedBy = UserAccessor.GetUsername();
            book.ModifiedBy = UserAccessor.GetUsername();
            await UnitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .CreateAsync(book, cancellationToken);

            //Add images
            var images = PhotoAccessor.AddRangePhotoAsync(request.Images ?? new List<IFormFile>()).Result;
            if (images.Count != request.Images?.Count)
                return Result<Unit>.Failure(T["Image upload failed"]);
            await UnitOfWork.GetRepository<IProductImageRepository, Domain.Catalog.ProductImage, string>()
                .CreateRangeAsync(images.Select(x => new Domain.Catalog.ProductImage
                {
                    Id = x.PublicId,
                    ProductId = product.Id,
                    ImageUrl = x.Url
                }), cancellationToken);

            //Add genres
            var genreIds = request.GenreIds ?? new List<Guid>();
            var genres = await UnitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetAsync(x => genreIds.Contains(x.Id), cancellationToken: cancellationToken);
            if (genres.Count != genreIds.Count)
                return Result<Unit>.Failure(T["Genre not found"]);
            await UnitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
                .CreateRangeAsync(genres.Select(x => new BookGenre
                {
                    BookId = book.Id,
                    GenreId = x.Id
                }), cancellationToken);

            //Save changes
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(T["Book created successfully"]);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateBookRequestHandler), e.Message, e);
        }
    }
}