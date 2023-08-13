using AutoMapper;
using FirstBlazorProject_BookStore.Common.Core;
using FirstBlazorProject_BookStore.Common.DTOs;
using FirstBlazorProject_BookStore.Common.DTOs.Demo;
using FirstBlazorProject_BookStore.Repository.Unit;
using Microsoft.Extensions.Logging;

namespace FirstBlazorProject_BookStore.Service.Demo;

public class DemoService : IDemoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DemoService> _logger;
    private readonly IMapper _mapper;

    public DemoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DemoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<DemoDto>> GetDemoByIdAsync(Guid id)
    {
        try
        {
            var demo = await _unitOfWork.GetRepository<DataAccess.Entities.Demo, Guid>().GetByIdAsync(id);
            if (demo == null)
            {
                return Result<DemoDto>.Failure(404, $"Demo with id {id} does not exist.");
            }
            var demoDto = _mapper.Map<DemoDto>(demo);
            return Result<DemoDto>.Success(demoDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result<DemoDto>.Failure(e.Message);
        }
    }

    public async Task<Result<DemoInputDto>> CreateAsync(DemoInputDto demoInputDto)
    {
        try
        {
            var demo = _mapper.Map<DataAccess.Entities.Demo>(demoInputDto);
            await _unitOfWork.GetRepository<DataAccess.Entities.Demo, Guid>().CreateAsync(demo);
            await _unitOfWork.SaveChangesAsync();
            demoInputDto = _mapper.Map<DemoInputDto>(demo);
            return Result<DemoInputDto>.Success(demoInputDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result<DemoInputDto>.Failure(e.Message);
        }
    }
}