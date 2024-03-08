using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Catalog.Product;

public class CreateProductRequest : IRequest<Result<Unit>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal SalePrice { get; set; }
    public string? Currency { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }
    public List<IFormFile>? Images { get; set; }
}

public class CreateProductRequestValidator<TProductCreateRequest> : CustomValidator<TProductCreateRequest>
    where TProductCreateRequest : CreateProductRequest
{
    protected CreateProductRequestValidator(IStringLocalizer<CreateProductRequestValidator<TProductCreateRequest>> t) :
        base(t)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(t["Title is required"])
            .MaximumLength(200).WithMessage(t["Title must not exceed {0} characters", 200]);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(t["Description is required"])
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.ImportPrice)
            .NotEmpty().WithMessage(t["ImportPrice is required"])
            .GreaterThan(0).WithMessage(t["ImportPrice must be greater than {0}", 0]);
        RuleFor(x => x.SalePrice)
            .NotEmpty().WithMessage(t["SalePrice is required"])
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

public class CreateProductRequestHandler<TProductCreateRequest> :
    IRequestHandler<TProductCreateRequest, Result<Unit>> where TProductCreateRequest : CreateProductRequest
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;
    protected readonly IStringLocalizer T;
    protected readonly IUserAccessor UserAccessor;
    protected readonly IPhotoAccessor PhotoAccessor;
    protected readonly ILogger<CreateProductRequestHandler<TProductCreateRequest>> Logger;

    protected CreateProductRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        Logger = loggerFactory.CreateLogger<CreateProductRequestHandler<TProductCreateRequest>>();
        T = t.Create(typeof(CreateProductRequestHandler<TProductCreateRequest>));
        UserAccessor = infrastructureServiceManager.UserAccessor;
        PhotoAccessor = infrastructureServiceManager.PhotoAccessor;
    }


    public virtual async Task<Result<Unit>> Handle(TProductCreateRequest request, CancellationToken cancellationToken)
    {
        //override this method in the derived class
        return await Task.FromResult(Result<Unit>.Success(T["Product created successfully"]));
    }
}