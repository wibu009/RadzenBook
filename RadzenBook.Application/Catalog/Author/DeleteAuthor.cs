using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Catalog.Author;

public class DeleteAuthorRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteAuthorRequestHandler : IRequestHandler<DeleteAuthorRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly ILogger<DeleteAuthorRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public DeleteAuthorRequestHandler(
        IUnitOfWork unitOfWork,
        ILoggerFactory logger,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager)
    {
        _unitOfWork = unitOfWork;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
        _logger = logger.CreateLogger<DeleteAuthorRequestHandler>();
        _t = t.Create(typeof(DeleteAuthorRequestHandler));
    }

    public async Task<Result<Unit>> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var author = await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (author is null)
            {
                return Result<Unit>.Failure(_t["Author not found."]);
            }

            if (!string.IsNullOrEmpty(author.ImageUrl))
            {
                await _photoAccessor.DeletePhotoByUrlAsync(author.ImageUrl);
            }

            await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .DeleteAsync(author, cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Author deleted successfully."]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteAuthorRequestHandler), e.Message, e);
        }
    }
}