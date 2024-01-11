using RadzenBook.Domain.Catalog;

namespace RadzenBook.Domain.Sales;

public class Cart : BaseEntity<Guid>
{
    public decimal TotalPrice { get; set; }
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public virtual ICollection<CartItem> Items { get; set; } = new HashSet<CartItem>();
}