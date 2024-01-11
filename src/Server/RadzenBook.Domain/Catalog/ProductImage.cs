namespace RadzenBook.Domain.Catalog;

public class ProductImage : BaseEntity<string>
{
    public string? ImageUrl { get; set; }
    public bool IsMain { get; set; } = false;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
}