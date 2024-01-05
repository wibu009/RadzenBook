namespace RadzenBook.Domain.Catalog;

public class Order : BaseEntity<Guid>
{
    public string? Note { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? Paid { get; set; }
    public DateTime? Shipped { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Discount { get; set; }
    public decimal? TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public Guid? ShippingAddressId { get; set; }
    public virtual CustomerAddress? ShippingAddress { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    public virtual ICollection<OrderProgress> Progresses { get; set; } = new HashSet<OrderProgress>();
}