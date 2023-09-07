namespace FirstBlazorProject_BookStore.Model.DTOs;

public class DemoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public class DemoInputDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}