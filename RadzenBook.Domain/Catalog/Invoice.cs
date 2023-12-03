namespace RadzenBook.Domain.Catalog;

public class Invoice : BaseEntity<Guid>
{
    public string? TrackingNumber { get; set; }
    public string? TrackingUrl { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Discount { get; set; }
    public decimal? TotalPrice { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = default!;
    public Guid PaymentMethodId { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; } = default!;
}