namespace RadzenBook.Domain.Catalog;

public class CustomerDiscountCode : BaseEntity<Guid>
{
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public Guid? DiscountCodeId { get; set; }
    public virtual DiscountCode? DiscountCode { get; set; }
    public CustomerDiscountCodeStatus Status { get; set; }
}