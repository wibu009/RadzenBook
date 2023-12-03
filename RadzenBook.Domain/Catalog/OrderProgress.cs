namespace RadzenBook.Domain.Catalog;

public class OrderProgress : BaseEntity<Guid>
{
    public string? Note { get; set; }
    public OrderStatus Status { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = default!;
}