using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO;
using RadzenBook.Contract.DTO.Demo;

namespace RadzenBook.Service.Interfaces.Features;

public interface IDemoService
{
    Task<Result<PaginatedList<DemoDto>>> GetAllDemosAsync();
    Task<Result<PaginatedList<DemoDto>>> GetPagedDemosAsync(PagingParams pagingParams);
    Task<Result<DemoDto>> GetDemoByIdAsync(Guid id);
    Task<Result<Unit>> CreateDemoAsync(DemoCreateDto demoCreateDto);
    Task<Result<Unit>> UpdateDemoAsync(Guid id, DemoUpdateDto demoUpdateDto);
    Task<Result<Unit>> DeleteDemoAsync(Guid id);
}