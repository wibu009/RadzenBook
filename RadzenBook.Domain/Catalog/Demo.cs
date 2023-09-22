using RadzenBook.Domain.Common.Contracts;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Domain.Catalog;

public class Demo : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public DemoEnum DemoEnum { get; set; } = DemoEnum.Demo1;
}