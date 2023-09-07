using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO;
using RadzenBook.Entity;
using RadzenBook.Repository.Implements;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Interfaces.Features;

namespace RadzenBook.Service.Implements.Features;

public class DemoService : IDemoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDemoRepository _demoRepository;
    private readonly ILogger<DemoService> _logger;
    private readonly IMapper _mapper;

    public DemoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DemoService> logger)
    {
        _unitOfWork = unitOfWork;
        _demoRepository = _unitOfWork.GetRepository<DemoRepository, Demo, Guid>();
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<DemoDto>> GetDemoByIdAsync(Guid id)
    {
        try
        {
            var demo = await _demoRepository.GetByIdAsync(id);
            if (demo == null)
            {
                return Result<DemoDto>.Failure($"Demo with id {id} does not exist.");
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
            var demo = _mapper.Map<Demo>(demoInputDto);
            await _demoRepository.DeleteAsync(demo);
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