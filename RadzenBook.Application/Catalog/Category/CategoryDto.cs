namespace RadzenBook.Application.Catalog.Category;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int TotalProducts { get; set; }
}