using RadzenBook.Domain.Enums;

namespace RadzenBook.Domain.Entities;

public class Demo : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public DemoEnum DemoEnum { get; set; } = DemoEnum.Demo1;
}