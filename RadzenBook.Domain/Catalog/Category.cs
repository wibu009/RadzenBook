namespace RadzenBook.Domain.Catalog;

public class Category : BaseEntity<Guid>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
}