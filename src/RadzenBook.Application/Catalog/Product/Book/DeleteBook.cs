using RadzenBook.Domain.Sales;

namespace RadzenBook.Application.Catalog.Product.Book;

public class DeleteBookRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteBookRequestValidator : CustomValidator<DeleteBookRequest>
{
    public DeleteBookRequestValidator(IStringLocalizer<DeleteBookRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"]);
    }
}

public class DeleteBookRequestHandler : IRequestHandler<DeleteBookRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteBookRequestHandler> _logger;

    public DeleteBookRequestHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _logger = loggerFactory.CreateLogger<DeleteBookRequestHandler>();
        _t = t.Create(typeof(DeleteBookRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the book from the database
            var product = await _unitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .GetByIdAsync(request.Id, includeProperties: "Book", cancellationToken: cancellationToken);

            // Check if the book exists
            if (product == null)
                return Result<Unit>.Failure(_t["Book not found"]);

            // Check if the book is already deleted
            if (product.IsDeleted)
                return Result<Unit>.Failure(_t["Book not found"]);

            // Delete Product
            await _unitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .SoftDeleteAsync(product, cancellationToken);

            // Delete Book
            await _unitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == request.Id, cancellationToken);

            //Delete Product Images
            await _unitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Product Discounts
            await _unitOfWork.GetRepository<IProductDiscountRepository, ProductDiscount, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Cart Items
            await _unitOfWork.GetRepository<ICartItemRepository, CartItem, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Reviews
            await _unitOfWork.GetRepository<IReviewRepository, Review, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            // Return a success result
            return Result<Unit>.Success(_t["Book deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteBookRequestHandler), e.Message, e);
        }
    }
}