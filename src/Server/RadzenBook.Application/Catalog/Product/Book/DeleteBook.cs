using RadzenBook.Domain.Sales;

namespace RadzenBook.Application.Catalog.Product.Book;

public class DeleteBookRequest : DeleteProductRequest
{
}

public class DeleteBookRequestValidator : DeleteProductRequestValidator<DeleteBookRequest>
{
    public DeleteBookRequestValidator(IStringLocalizer<DeleteBookRequestValidator> t) : base(t)
    {
    }
}

public class DeleteBookRequestHandler : DeleteProductRequestHandler<DeleteBookRequest>
{
    protected DeleteBookRequestHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IStringLocalizerFactory t, 
        IInfrastructureServiceManager infrastructureServiceManager, 
        ILoggerFactory loggerFactory) : 
        base(unitOfWork, mapper, t, infrastructureServiceManager, loggerFactory)
    {
    }
    
    public override async Task<Result<Unit>> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the book from the database
            var product = await UnitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .GetByIdAsync(request.Id, includeProperties: "Book", cancellationToken: cancellationToken);

            // Check if the book exists
            if (product == null)
                return Result<Unit>.Failure(T["Book not found"]);

            // Check if the book is already deleted
            if (product.IsDeleted)
                return Result<Unit>.Failure(T["Book not found"]);

            // Delete Product
            await UnitOfWork.GetRepository<IProductRepository, Domain.Catalog.Product, Guid>()
                .SoftDeleteAsync(product, cancellationToken);

            // Delete Book
            await UnitOfWork.GetRepository<IBookRepository, Domain.Catalog.Book, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == request.Id, cancellationToken);

            //Delete Product Images
            await UnitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Product Discounts
            await UnitOfWork.GetRepository<IProductDiscountRepository, ProductDiscount, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Cart Items
            await UnitOfWork.GetRepository<ICartItemRepository, CartItem, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            //Delete Reviews
            await UnitOfWork.GetRepository<IReviewRepository, Review, Guid>()
                .SoftDeleteByPropsAsync(x => x.ProductId == product.Id, cancellationToken);

            // Return a success result
            return Result<Unit>.Success(T["Book deleted successfully"]);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteBookRequestHandler), e.Message, e);
        }
    }
}