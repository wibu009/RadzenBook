using RadzenBook.Application.Common.Photo;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Product;

public class UpdateProductRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string? Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public List<string>? ImageIds { get; set; }
    public List<IFormFile>? NewImages { get; set; }
}

public class UpdateProductRequestValidator<TUpdateProductRequest> : CustomValidator<TUpdateProductRequest> 
    where TUpdateProductRequest : UpdateProductRequest
{
    protected UpdateProductRequestValidator(IStringLocalizer<UpdateProductRequestValidator<TUpdateProductRequest>> t) : base(t)
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage(t["Title must not exceed {0} characters", 200]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.ImportPrice)
            .GreaterThan(0).WithMessage(t["ImportPrice must be greater than {0}", 0]);
        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0).WithMessage(t["SalePrice must be greater than or equal to {0}", 0]);
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage(t["Currency is required"]);
        RuleFor(x => x.UnitPrice)
            .NotEmpty().WithMessage(t["UnitPrice is required"])
            .GreaterThanOrEqualTo(0).WithMessage(t["UnitPrice must be greater than or equal to {0}", 0]);
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(t["Quantity is required"])
            .GreaterThanOrEqualTo(0).WithMessage(t["Quantity must be greater than or equal to {0}", 0]);
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(t["Category is required"]);
    }
}

public class UpdateProductRequestHandler<TUpdateProductRequest> : IRequestHandler<TUpdateProductRequest, Result<Unit>>
    where TUpdateProductRequest : UpdateProductRequest
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;
    protected readonly IStringLocalizer T;
    protected readonly IUserAccessor UserAccessor;
    protected readonly IPhotoAccessor PhotoAccessor;
    protected readonly ILogger<UpdateProductRequestHandler<TUpdateProductRequest>> Logger;

    protected UpdateProductRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        IUserAccessor userAccessor,
        IPhotoAccessor photoAccessor,
        ILogger<UpdateProductRequestHandler<TUpdateProductRequest>> logger)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        T = t.Create(typeof(UpdateProductRequestValidator<TUpdateProductRequest>));
        UserAccessor = userAccessor;
        PhotoAccessor = photoAccessor;
        Logger = logger;
    }

    public virtual async Task<Result<Unit>> Handle(TUpdateProductRequest request, CancellationToken cancellationToken)
    {
        //override this method in derived class
        return await Task.FromResult(Result<Unit>.Success(T["Product updated successfully"]));
    }
    
    protected async Task UpdateImages(TUpdateProductRequest request, Domain.Catalog.Product product,
        CancellationToken cancellationToken)
    {
        // Add new images if they are provided
        if (request.NewImages is { Count: > 0 })
        {
            var newImages = await PhotoAccessor.AddRangePhotoAsync(request.NewImages);
            var imagesToAdd = Mapper.Map<List<ProductImage>>(newImages);
            await UnitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
                .CreateRangeAsync(imagesToAdd, cancellationToken);
        }

        // Retrieve the existing images from the database
        var existingImages = await UnitOfWork
            .GetRepository<IProductImageRepository, ProductImage, string>()
            .GetAsync(x => x.ProductId == product.Id, cancellationToken: cancellationToken);

        // Convert the existing images and request images to sets for easy comparison
        var requestImageIds = new HashSet<string>(request.ImageIds ?? new List<string>());

        var imagesToDelete = existingImages.Where(x => !requestImageIds.Contains(x.Id)).ToList();

        // Delete removed images
        await PhotoAccessor.DeleteRangePhotoAsync(imagesToDelete.Select(x => x.Id).ToList());
        await UnitOfWork.GetRepository<IProductImageRepository, ProductImage, string>()
            .DeleteRangeAsync(imagesToDelete, cancellationToken);
    }
}