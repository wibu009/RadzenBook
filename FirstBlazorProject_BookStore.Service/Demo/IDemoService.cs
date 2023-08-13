using FirstBlazorProject_BookStore.Common.Core;
using FirstBlazorProject_BookStore.Common.DTOs;
using FirstBlazorProject_BookStore.Common.DTOs.Demo;

namespace FirstBlazorProject_BookStore.Service.Demo;

public interface IDemoService
{
    public Task<Result<DemoDto>> GetDemoByIdAsync(Guid id);
    public Task<Result<DemoInputDto>> CreateAsync(DemoInputDto demoInputDto);
}