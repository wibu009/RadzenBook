namespace RadzenBook.Domain.Catalog;

public class CustomerDiscountCode : BaseEntity<Guid>
{
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public Guid DiscountCodeId { get; set; }
    public virtual DiscountCode DiscountCode { get; set; } = default!;
    public CustomerDiscountCodeStatus Status { get; set; }
}