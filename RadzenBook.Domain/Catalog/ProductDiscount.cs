namespace RadzenBook.Domain.Catalog;

public class ProductDiscount : BaseEntity<Guid>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DiscountType DiscountType { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? UsageLimit { get; set; }
    public bool IsEnabled { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
}