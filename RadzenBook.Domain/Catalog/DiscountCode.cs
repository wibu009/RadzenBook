namespace RadzenBook.Domain.Catalog;

public class DiscountCode : BaseEntity<Guid>
{
    public string Code { get; set; } = default!;
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public decimal MinimumPurchaseAmount { get; set; }
}