using RadzenBook.Application.Common.Photo;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product.Book;

public class UpdateBookRequest : UpdateProductRequest
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

public class UpdateBookRequestValidator : UpdateProductRequestValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator(IStringLocalizer<UpdateBookRequestValidator> t) : base(t)
    {
        RuleFor(x => x.ISBN)
            .MaximumLength(50).WithMessage(t["ISBN must not exceed {0} characters", 50]);
        RuleFor(x => x.Language)
            .MaximumLength(50).WithMessage(t["Language must not exceed {0} characters", 50]);
        RuleFor(x => x.Translator)
            .MaximumLength(50).WithMessage(t["Translator must not exceed {0} characters", 50]);
        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage(t["PageCount must be greater than {0}", 0]);
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

public class UpdateBookRequestHandler : UpdateProductRequestHandler<UpdateBookRequest>
{
    public UpdateBookRequestHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IStringLocalizerFactory t, 
        IInfrastructureServiceManager infrastructureServiceManager, 
        IUserAccessor userAccessor, 
        IPhotoAccessor photoAccessor, ILogger<UpdateProductRequestHandler<UpdateBookRequest>> logger) : base(unitOfWork, mapper, t, infrastructureServiceManager, userAccessor, photoAccessor, logger)
    {
    }
    
    public override async Task<Result<Unit>> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the existing product
            var product = await UnitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (product == null)
            {
                return Result<Unit>.Failure(T["Product does not exist"]);
            }

            // Update product properties
            Mapper.Map(request, product);
            product.ModifiedBy = UserAccessor.GetUsername();

            // Get the existing book
            var book = await UnitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (book == null)
            {
                return Result<Unit>.Failure(T["Book does not exist"]);
            }

            // Update book properties
            Mapper.Map(request, book);
            book.ModifiedBy = UserAccessor.GetUsername();

            // Update images
            await UpdateImages(request, product, cancellationToken);

            // Update genres
            await UpdateGenres(request, book, cancellationToken);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(T["Book updated successfully"]);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateBookRequestHandler), e.Message, e);
        }
    }

    private async Task UpdateGenres(UpdateBookRequest request, Domain.Catalog.Book book,
        CancellationToken cancellationToken)
    {
        // Retrieve the existing genres from the database
        var existingGenres = await UnitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
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

        await UnitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
            .CreateRangeAsync(newGenres, cancellationToken);

        // Delete removed genres
        await UnitOfWork.GetRepository<IBookGenreRepository, BookGenre, Guid>()
            .DeleteRangeAsync(genresToDelete, cancellationToken);
    }
}