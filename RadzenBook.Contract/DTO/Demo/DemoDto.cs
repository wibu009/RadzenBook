namespace RadzenBook.Contract.DTO.Demo;

public class DemoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DemoEnum { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime ModifiedAt { get; set; } = default!;
}