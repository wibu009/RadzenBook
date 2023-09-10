using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenBook.Common.Exceptions;
using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO.Demo;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Implements.Features;

public class DemoService : IDemoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDemoRepository _demoRepository;
    private readonly ILogger<DemoService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public DemoService(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<DemoService> logger,
        IInfrastructureServiceManager infrastructureServiceManager)
    {
        _unitOfWork = unitOfWork;
        _demoRepository = _unitOfWork.GetRepository<IDemoRepository, Demo, Guid>();
        _mapper = mapper;
        _logger = logger;
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<PaginatedList<DemoDto>>> GetAllDemosAsync()
    {
        try
        {
            var demos = await _demoRepository.GetAsync();
            var size = demos.Count;
            var demosDto = _mapper.Map<List<DemoDto>>(demos);
            var demosDtoPaginated = await PaginatedList<DemoDto>.CreateAsync(demosDto, 1, size);
            return Result<PaginatedList<DemoDto>>.Success(demosDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(GetAllDemosAsync), nameof(DemoService), e.Message, e);
        }
    }

    public async Task<Result<PaginatedList<DemoDto>>> GetPagedDemosAsync(PagingParams pagingParams)
    {
        try
        {
            var demos = await _demoRepository.GetPagedAsync(pageNumber: pagingParams.PageNumber, pageSize: pagingParams.PageSize);
            var count = await _demoRepository.CountAsync();
            var demosDto = _mapper.Map<List<DemoDto>>(demos);
            var demosDtoPaginated = new PaginatedList<DemoDto>(demosDto, count, pagingParams.PageNumber, pagingParams.PageSize);
            return Result<PaginatedList<DemoDto>>.Success(demosDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(GetPagedDemosAsync), nameof(DemoService), e.Message, e);
        }
    }

    public async Task<Result<DemoDto>> GetDemoByIdAsync(Guid id)
    {
        try
        {
            var demo = await _demoRepository.GetByIdAsync(id);
            if (demo == null)
            {
                return Result<DemoDto>.Failure($"Demo with id {id} does not exist.", (int)HttpStatusCode.NotFound);
            }
            var demoDto = _mapper.Map<DemoDto>(demo);
            return Result<DemoDto>.Success(demoDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(GetDemoByIdAsync), nameof(DemoService), e.Message, e);
        }
    }

    public async Task<Result<Unit>> CreateDemoAsync(DemoCreateDto demoCreateDto)
    {
        try
        {
            var demo = _mapper.Map<Demo>(demoCreateDto);
            demo.CreatedBy = _userAccessor.GetUsername();
            demo.ModifiedBy = _userAccessor.GetUsername();
            await _demoRepository.CreateAsync(demo);
            await _unitOfWork.SaveChangesAsync();
            return Result<Unit>.Success("Create demo successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(CreateDemoAsync), nameof(DemoService), e.Message, e);
        }
    }

    public async Task<Result<Unit>> UpdateDemoAsync(Guid id, DemoUpdateDto demoUpdateDto)
    {
        try
        {
            var demo = await _demoRepository.GetByIdAsync(id);
            if (demo == null)
            {
                return Result<Unit>.Failure($"Demo with id {id} does not exist.", (int)HttpStatusCode.NotFound);
            }
            _mapper.Map(demoUpdateDto, demo);
            demo.ModifiedBy = _userAccessor.GetUsername();
            await _demoRepository.UpdateAsync(demo);
            await _unitOfWork.SaveChangesAsync();
            return Result<Unit>.Success("Update demo successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(UpdateDemoAsync), nameof(DemoService), e.Message, e);
        }
    }

    public async Task<Result<Unit>> DeleteDemoAsync(Guid id)
    {
        try
        {
            var demo = await _demoRepository.GetByIdAsync(id);
            if (demo == null)
            {
                return Result<Unit>.Failure($"Demo with id {id} does not exist.", (int)HttpStatusCode.NotFound);
            }
            await _demoRepository.DeleteAsync(demo);
            await _unitOfWork.SaveChangesAsync();
            return Result<Unit>.Success("Delete demo successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(DeleteDemoAsync), nameof(DemoService), e.Message, e);
        }
    }
}