using RadzenBook.Contract.Core;
using RadzenBook.Contract.DTO;

namespace RadzenBook.Service.Interfaces.Features;

public interface IDemoService
{
    public Task<Result<DemoDto>> GetDemoByIdAsync(Guid id);
    public Task<Result<DemoInputDto>> CreateAsync(DemoInputDto demoInputDto);
}