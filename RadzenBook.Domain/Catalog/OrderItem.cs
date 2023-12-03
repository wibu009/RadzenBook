namespace RadzenBook.Domain.Catalog;

public class OrderItem : BaseEntity<Guid>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = default!;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = default!;
}