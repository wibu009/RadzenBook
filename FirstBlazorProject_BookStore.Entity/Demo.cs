using FirstBlazorProject_BookStore.Model.Enums;

namespace FirstBlazorProject_BookStore.Entity;

public class Demo : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public DemoEnum DemoEnum { get; set; } = DemoEnum.Demo1;
}