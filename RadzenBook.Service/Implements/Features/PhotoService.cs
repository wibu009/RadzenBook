using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Photo;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Implements.Features;

public class PhotoService : IPhotoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPhotoRepository _photoRepository;
    private readonly ILogger<PhotoService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;
    private readonly IPhotoAccessor _photoAccessor;
    
    public PhotoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PhotoService> logger,
        IInfrastructureServiceManager infrastructureServiceManager)
    {
        _unitOfWork = unitOfWork;
        _photoRepository = _unitOfWork.GetRepository<IPhotoRepository, Photo, string>();
        _mapper = mapper;
        _logger = logger;
        _userAccessor = infrastructureServiceManager.UserAccessor;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
    }
}