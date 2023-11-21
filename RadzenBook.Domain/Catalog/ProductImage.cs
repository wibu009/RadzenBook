namespace RadzenBook.Domain.Catalog;

public class ProductImage : BaseEntity<string>
{
    public string Url { get; set; } = default!;
    public bool IsMain { get; set; } = false;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
}