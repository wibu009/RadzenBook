using FirstBlazorProject_BookStore.Model.Core;
using FirstBlazorProject_BookStore.Model.Cores;
using FirstBlazorProject_BookStore.Model.DTOs.Demo;

namespace FirstBlazorProject_BookStore.Service.Interfaces.Features;

public interface IDemoService
{
    public Task<Result<DemoDto>> GetDemoByIdAsync(Guid id);
    public Task<Result<DemoInputDto>> CreateAsync(DemoInputDto demoInputDto);
}