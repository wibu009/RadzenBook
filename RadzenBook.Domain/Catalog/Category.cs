namespace RadzenBook.Domain.Catalog;

public class Category : BaseEntity<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}