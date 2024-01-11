using RadzenBook.Domain.Catalog;

namespace RadzenBook.Domain.Sales;

public class CustomerDiscountCode : BaseEntity<Guid>
{
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public Guid? DiscountCodeId { get; set; }
    public virtual DiscountCode? DiscountCode { get; set; }
    public CustomerDiscountCodeStatus Status { get; set; }
}