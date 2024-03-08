using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Catalog.Product;

public class DeleteProductRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteProductRequestValidator<TDeleteProductRequest> : CustomValidator<TDeleteProductRequest>
    where TDeleteProductRequest : DeleteProductRequest
{
    protected DeleteProductRequestValidator(IStringLocalizer<DeleteProductRequestValidator<TDeleteProductRequest>> t) :
        base(t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"]);
    }
}

public class DeleteProductRequestHandler<TDeleteProductRequest> : IRequestHandler<TDeleteProductRequest, Result<Unit>>
    where TDeleteProductRequest : DeleteProductRequest
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;
    protected readonly IStringLocalizer T;
    protected readonly IUserAccessor UserAccessor;
    protected readonly IPhotoAccessor PhotoAccessor;
    protected readonly ILogger<DeleteProductRequestHandler<TDeleteProductRequest>> Logger;

    protected DeleteProductRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        UserAccessor = infrastructureServiceManager.UserAccessor;
        PhotoAccessor = infrastructureServiceManager.PhotoAccessor;
        T = t.Create(typeof(DeleteProductRequestHandler<TDeleteProductRequest>));
        Logger = loggerFactory.CreateLogger<DeleteProductRequestHandler<TDeleteProductRequest>>();
    }

    public virtual async Task<Result<Unit>> Handle(TDeleteProductRequest request, CancellationToken cancellationToken)
    {
        //override this method in derived class
        return await Task.FromResult(Result<Unit>.Success(T["Product deleted successfully"]));
    }
}