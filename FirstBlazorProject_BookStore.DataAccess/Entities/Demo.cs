using FirstBlazorProject_BookStore.DataAccess.Enum;

namespace FirstBlazorProject_BookStore.DataAccess.Entities;

public class Demo : BaseEntity<Guid>
{
    public string Name { get; set; }
    public DemoEnum DemoEnum { get; set; } = DemoEnum.Demo1;
}