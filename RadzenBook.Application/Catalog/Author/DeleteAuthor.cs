﻿using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Catalog.Author;

public class DeleteAuthorRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteAuthorRequestHandler : IRequestHandler<DeleteAuthorRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteAuthorRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public DeleteAuthorRequestHandler(
        IUnitOfWork unitOfWork,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager)
    {
        _unitOfWork = unitOfWork;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
        _userAccessor = infrastructureServiceManager.UserAccessor;
        _logger = loggerFactory.CreateLogger<DeleteAuthorRequestHandler>();
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
                return Result<Unit>.Failure(_t["Author not found"]);
            }

            author.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .SoftDeleteAsync(author, cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Author deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteAuthorRequestHandler), e.Message, e);
        }
    }
}