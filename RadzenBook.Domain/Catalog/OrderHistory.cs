namespace RadzenBook.Domain.Catalog;

public class OrderHistory : BaseEntity<Guid>
{
    public string TrackingNumber { get; set; } = default!;
    public OrderStatus Status { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = default!;
}